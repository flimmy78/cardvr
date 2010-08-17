using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video.VFW;
using AForge.Video.DirectShow;
using AForge.Video;
using System.IO;
using System.Threading;
using System.Reflection;
using Microsoft.Win32;

namespace CarDVR
{
    enum ButtonState
    {
        Start,
        Stop
    }

    public partial class MainForm : Form
    {
        VideoCaptureDevice videoSource;
        GpsReciever gps;
        System.Windows.Forms.Timer timerSplit;

        private ButtonState buttonState = ButtonState.Start;

        private static readonly int camWidth = 640;
        private static readonly int camHeight = 480;
        private static readonly int fps = 24;

        private int secondsElapsed = 0;
        private bool nextAviPrepared = false;

        private object secondsWatchDog = new object();

        AVIWriter[] avis = new AVIWriter[2];
        object aviWatchDog = new object();

        public MainForm()
        {
            Program.settings.Read();

            avis[0] = new AVIWriter("XVID");
            avis[0].FrameRate = fps;

            avis[1] = new AVIWriter("XVID");
            avis[1].FrameRate = fps;

            gps = new GpsReciever();

			if (Program.settings.GpsEnabled)
				gps.Initialize(Program.settings.GpsSerialPort, Program.settings.SerialPortBaudRate);

            // create first video source
			videoSource = new VideoCaptureDevice(Program.settings.VideoSourceId);
            videoSource.DesiredFrameRate = fps;
            videoSource.DesiredFrameSize = new System.Drawing.Size(camWidth, camHeight);
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);

            InitializeComponent();

			if (Program.settings.StartMinimized)
				buttonMinimize_Click(this, EventArgs.Empty);

            buttonState = ButtonState.Start;

            IsWebCamAvaliable();

            timerSplit = new System.Windows.Forms.Timer();
            timerSplit.Interval = 1000;
            timerSplit.Tick += new EventHandler(timerSplit_Tick);

			if (Program.settings.AutostartRecording && !string.IsNullOrEmpty(Program.settings.VideoSource))
				buttonStartStop_Click(this, EventArgs.Empty);
        }

        void timerSplit_Tick(object sender, EventArgs e)
        {
            lock (secondsWatchDog)
            {
                ++secondsElapsed;
            }
        }

		private class FileInfoComparer : IComparer<FileInfo>
		{
			public int Compare(FileInfo x, FileInfo y)
			{
				if (x.CreationTime == y.CreationTime)
					return 0;

				if (x.CreationTime > y.CreationTime)
					return -1;

				if (x.CreationTime < y.CreationTime)
					return 1;
				
				return 0;
			}
		}

        private void PrepareNewMovie()
        {
            StartNewMovie(1);
        }

		public void CloseCurrentAvi()
		{
			avis[0].Close();
		}
        
        public void CloseOldAvi()
        {
            avis[1].Close();
        }

        private void StartNewMovie(int oneOfAvi)
        {
            // if preparing, next avi will started after 10 seconds
            string filename = oneOfAvi == 1 ? DateTime.Now.AddSeconds(9).ToString() : DateTime.Now.ToString();

			filename = Program.settings.PathForVideo + "\\CarDVR_" +
                       filename.Replace(':', '_').Replace(' ', '_').Replace('.', '_');

            avis[oneOfAvi].Open(filename + ".avi", camWidth, camHeight);

            DirectoryInfo dir = new DirectoryInfo(Program.settings.PathForVideo);
			FileInfo[] files = dir.GetFiles("*.avi");

			Array.Sort<FileInfo>(files, new FileInfoComparer());

			for (int index = Program.settings.AmountOfFiles; index < files.Length; ++index) 
			{
				try
				{
					File.Delete(files[index].FullName);
				}
				catch (Exception) { }
			}
        }

		private string MakeFrameString()
        {
            string result = DateTime.Now.ToString() + " ";

            // Do not write anything if GPS disabled in settings
			if (Program.settings.GpsEnabled)
            {
                switch (gps.GpsState)
                {
                    case GpsState.Active:
                        result += "Скорость: " + gps.Speed.ToString() + " км/ч Cпутников: " + gps.NumberOfSattelites.ToString() + "\n" + gps.Coordinates;
                        break;
                    case GpsState.NoSignal:
                        result += "Нет сигнала GPS";
                        break;
                    case GpsState.NotActive:
                        result += "GPS не подключен";
                        break;
                }
            }

            return result;
        }

        void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            int localSeconds;
            lock (secondsWatchDog)
            {
                localSeconds = secondsElapsed;
            }

            // before 10 seconds, open new avi
			if (!nextAviPrepared && (localSeconds % (Program.settings.AviDuration * 60)) == (Program.settings.AviDuration * 60 - 9))
            {
                nextAviPrepared = true;
                new Thread(PrepareNewMovie).Start();
            }

			if (nextAviPrepared && (localSeconds % (Program.settings.AviDuration * 60)) == 0)
            {
                nextAviPrepared = false;
                lock (aviWatchDog)
                {
                    AVIWriter tmp = avis[0];
                    avis[0] = avis[1];
                    avis[1] = tmp;
                }
				new Thread(CloseOldAvi).Start();
            }

            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            {
                Graphics picasso = Graphics.FromImage(frame);
                picasso.DrawString(MakeFrameString(), new Font("Arial", 8, FontStyle.Bold), Brushes.White, new Point(5, 5));
            }

            lock (aviWatchDog)
            {
                avis[0].AddFrame(frame);
            }
            camView.Image = frame;
        }

        private bool IsWebCamAvaliable()
        {
			bool WebCamPresent = !string.IsNullOrEmpty(Program.settings.VideoSource);

            buttonStartStop.Enabled = WebCamPresent;
            labelNoVideoSource.Visible = !WebCamPresent;

            return WebCamPresent;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            using (settingsForm settingsForm = new settingsForm())
            {
                settingsForm.LoadFromRegistry();
                settingsForm.ApplySettingsToForm();

                settingsForm.ShowDialog();

                settingsForm.ApplyFormToSettings();
                settingsForm.SaveToRegistry();

				// apply some parameters immediately
				if (Program.settings.StartWithWindows)
					AutorunHelper.EnableAutorun();
				else
					AutorunHelper.DisableAutorun();
            }            
        }

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            buttonStartStop.Enabled = false;

            switch (buttonState)
            {
                case ButtonState.Start:
                    // Update settings, may be web cam became not avaliable
					Program.settings.Read();

                    if (!IsWebCamAvaliable())
                        return;

					if (Program.settings.GpsEnabled)
                    {
						gps.Initialize(Program.settings.GpsSerialPort, Program.settings.SerialPortBaudRate);
                        gps.Open();
                    }

                    nextAviPrepared = false;

                    StartNewMovie(0);
                    videoSource.Start();
                    timerSplit.Start();
                    break;

                case ButtonState.Stop:
                    videoSource.Stop();
                    videoSource.WaitForStop();
                    timerSplit.Stop();

					CloseCurrentAvi();
					CloseOldAvi();

                    // check for opened Serial Port implemented inside Gps Reciever class
                    gps.Close();

                    break;
            }
            buttonStartStop.Text = buttonState == ButtonState.Start ? "Stop" : "Start";
            buttonState = buttonState == ButtonState.Start ? ButtonState.Stop : ButtonState.Start;
            buttonStartStop.Enabled = true;
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            videoSource.Stop();
            videoSource.WaitForStop();

			CloseCurrentAvi();
			CloseOldAvi();
        }

		private void buttonMinimize_Click(object sender, EventArgs e)
		{
			trayIcon.Visible = true;
			this.Hide();
		}

		private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.Show();
			trayIcon.Visible = false;
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}
    }
}
