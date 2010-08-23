using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video.VFW;
using AForge.Video.DirectShow;
using AForge.Video;
using System.IO;
using System.Threading;

namespace CarDVR
{
    public partial class MainForm : Form
    {
        private static readonly Font framefont = new Font("Arial", 8, FontStyle.Bold);
        private static readonly Point pointWhite = new Point(5, 5);
        private static readonly Point pointBlack = new Point(6, 6);
        private static ButtonState buttonState = ButtonState.Start;
		
        VideoCaptureDevice videoSource = null;
        GpsReciever gps;
		VideoSplitter splitter;

        private void InitVideoSource()
        {
            bool running = false;
            if (videoSource != null && videoSource.IsRunning)
            {
                running = true;
                buttonStartStop_Click(this, EventArgs.Empty);
            }

            if (videoSource != null)
                videoSource.NewFrame -= videoSource_NewFrame;

            videoSource = new VideoCaptureDevice(Program.settings.VideoSourceId);
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
            videoSource.DesiredFrameRate = Program.settings.VideoFps;
            videoSource.DesiredFrameSize = Program.settings.GetVideoSize();

            splitter.Codec = "XVID";
            //splitter.Codec = "FFDS";
            splitter.FPS = Program.settings.VideoFps;
            splitter.VideoSize = Program.settings.GetVideoSize();
            splitter.FileDuration = Program.settings.AviDuration;
            splitter.NumberOfFiles = Program.settings.AmountOfFiles;
            splitter.Path = Program.settings.PathForVideo;

            if (running)
                buttonStartStop_Click(this, EventArgs.Empty);
        }

        public MainForm()
        {
            Program.settings.Read();

            // Create avi-splitter. It will be initialized in InitVideoSource()
			splitter = new VideoSplitter();
            gps = new GpsReciever();

			if (Program.settings.GpsEnabled)
				gps.Initialize(Program.settings.GpsSerialPort, Program.settings.SerialPortBaudRate);

            // create first video source
            InitVideoSource();

            InitializeComponent();

            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Text += " v" + version.Substring(0, version.Length-4);

			if (Program.settings.StartMinimized)
				buttonMinimize_Click(this, EventArgs.Empty);

            buttonState = ButtonState.Start;

            IsWebCamAvaliable();

			if (Program.settings.AutostartRecording && !string.IsNullOrEmpty(Program.settings.VideoSource))
				buttonStartStop_Click(this, EventArgs.Empty);
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
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            Graphics graphics = Graphics.FromImage(frame);
            string frameString = MakeFrameString();
            graphics.DrawString(frameString, framefont, Brushes.Black, pointBlack);
            graphics.DrawString(frameString, framefont, Brushes.White, pointWhite);

			splitter.AddFrame(ref frame);

            if (Visible)
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

                if (settingsForm.ShowDialog() == DialogResult.Cancel)
                    return;

                settingsForm.ApplyFormToSettings();
                settingsForm.SaveToRegistry();

				// apply some parameters immediately
				if (Program.settings.StartWithWindows)
					AutorunHelper.EnableAutorun();
				else
					AutorunHelper.DisableAutorun();

                // reinit video source
                InitVideoSource();

                // reinit gps
                gps.Close();
                
                if (Program.settings.GpsEnabled)
                    gps.Initialize(Program.settings.GpsSerialPort, Program.settings.SerialPortBaudRate);

                gps.Open();
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

					splitter.Start();
                    videoSource.Start();
                    
                    break;

                case ButtonState.Stop:
                    videoSource.Stop();
                    videoSource.WaitForStop();
                    splitter.Stop();
                    camView.Image = new Bitmap(Program.settings.VideoWidth, Program.settings.VideoHeight);

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

            gps.Close();
			splitter.Stop();
        }

		private void buttonMinimize_Click(object sender, EventArgs e)
		{
			trayIcon.Visible = true;
			this.Hide();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void trayIcon_MouseClick(object sender, MouseEventArgs e)
		{
			this.Show();
			trayIcon.Visible = false;
		}
    }
}
