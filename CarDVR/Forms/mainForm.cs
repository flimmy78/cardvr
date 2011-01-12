using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video.VFW;
using AForge.Video.DirectShow;
using AForge.Video;
using System.IO;
using System.Threading;
using System.Resources;
using System.Globalization;

namespace CarDVR
{
	public partial class MainForm : Form
	{
		private static readonly Font framefont = new Font("Arial", 8, FontStyle.Bold);
		private static readonly Point pointWhite = new Point(5, 5);
		private static readonly Point pointBlack = new Point(6, 6);
		private static ButtonState buttonState = ButtonState.Start;
		private AutostartDelayer autostartDelayer;

		GpsReceiver gps = new GpsReceiver();
		VideoSplitter splitter = new VideoSplitter();

		VideoCaptureDevice videoSource = null;
		Bitmap frame;
		object frameKeeper = new object();

		public MainForm()
		{
			BeforeInitializeComponent();

			InitializeComponent();

			AfterInitializeComponent();
		}

		private bool IsRecording()
		{
			return videoSource != null && videoSource.IsRunning;
		}

		private void InitVideoSource()
		{
			// locking frameKeeper to prevent using video source
			lock (frameKeeper)
			{
				if (videoSource != null)
					videoSource.NewFrame -= videoSource_NewFrame;

				videoSource = new VideoCaptureDevice(Program.settings.VideoSourceId);
				videoSource.NewFrame += videoSource_NewFrame;
				videoSource.DesiredFrameRate = Program.settings.VideoFps;
				videoSource.DesiredFrameSize = new Size(Program.settings.VideoWidth, Program.settings.VideoHeight);

				splitter.Codec = Program.settings.Codec;
				splitter.FPS = Program.settings.OutputRateFps != 0 ? Program.settings.OutputRateFps : Program.settings.VideoFps;
				splitter.VideoSize = Program.settings.GetVideoSize();
				splitter.FileDuration = Program.settings.AviDuration;
				splitter.NumberOfFiles = Program.settings.AmountOfFiles;
				splitter.Path = Program.settings.PathForVideo;

				if (splitter.FPS > 0)
					timerWriter.Interval = 1000 / splitter.FPS;
			}
		}

		private void VideoInitialization()
		{
			InitVideoSource();

			if (Program.settings.AutostartRecording && !string.IsNullOrEmpty(Program.settings.VideoSource))
				StartStopRecording();
		}

		// TODO: make stand alone class FramesCounter
		int lastFrames = 0, totalFrames = 0, lastFps = 0;
		object framesCountKeeper = new object();

		private string MakeFrameString()
		{
			string result = DateTime.Now.ToString() + " ";

			if (Program.settings.GpsEnabled)
			{
				switch (gps.State)
				{
					case GpsState.Active:
						result +=	resSpeed + " " + gps.Speed + " " + 
									resKmh + " " + 
									resSatellites + " " + gps.NumberOfSatellites.ToString() + "\n" + gps.Coordinates;
						break;
					case GpsState.NoSignal:
						result += resNoGpsSignal;
						break;
					case GpsState.NotActive:
						result += resGpsNotConnected;
						break;
				}
			}

			lock (framesCountKeeper)
			{
				result += "\n" + totalFrames.ToString() + " | " + lastFps.ToString() + " FPS";
			}

			return result;
		}

		void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			++totalFrames;

			lock (frameKeeper)
			{
				frame = (Bitmap)eventArgs.Frame.Clone();

				if (Program.settings.EnableRotate)
				{
					switch (Program.settings.RotateAngle)
					{
						case 90:
							frame.RotateFlip(RotateFlipType.Rotate90FlipNone);
							break;
						case 180:
							frame.RotateFlip(RotateFlipType.Rotate180FlipNone);
							break;
						case 270:
							frame.RotateFlip(RotateFlipType.Rotate270FlipNone);
							break;
					}
				}

				using (Graphics graphics = Graphics.FromImage(frame))
				{
					string frameString = MakeFrameString();
					graphics.DrawString(frameString, framefont, Brushes.Black, pointBlack);
					graphics.DrawString(frameString, framefont, Brushes.White, pointWhite);
				}
			}
		}

		private bool IsWebCamAvaliable()
		{
			bool WebCamPresent;

			WebCamPresent = !string.IsNullOrEmpty(Program.settings.VideoSource);
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

				AutorunHelper.SetEnabled(Program.settings.StartWithWindows);

				IsWebCamAvaliable();

				// reinit video source
				{
					bool isRecording = IsRecording();

					if (isRecording)
						StopRecording();

					InitVideoSource();

					if (isRecording)
						StartRecording();
				}

				InitializeGps();
			}
		}

		private void InitializeGps()
		{
			try
			{
				if (Program.settings.GpsEnabled)
					gps.Initialize(Program.settings.GpsSerialPort, Program.settings.SerialPortBaudRate);
				else
					gps.Close();
			}
			catch (Exception e)
			{
				Reporter.NonSeriousError(e.Message);
			}
		}

		private void StartGpsReceiver()
		{
			if (!Program.settings.GpsEnabled)
				return;

			try
			{
				gps.Open();
			}
			catch (Exception e)
			{
				Reporter.NonSeriousError
				(
					string.Format
					(
						"Can't open GPS receiver on port '{0}'. GPS not active. Description: {1}",
						Program.settings.GpsSerialPort,
						e.Message
					)
				);
			}
		}

		private void StartRecording()
		{
            if (buttonState == ButtonState.Stop)
                return;

			buttonStartStop.Enabled = false;

			// Update settings, may be web cam became not avaliable
			Program.settings.Read();
			IsWebCamAvaliable();

			InitializeGps();
			StartGpsReceiver();		

			splitter.Start();
			videoSource.Start();
			FpsDisplayer.Enabled = true;

			buttonStartStop.Text = resStop;
			buttonState = ButtonState.Stop;

			buttonStartStop.Enabled = true;
		}

		private void StopRecording()
		{
            if (buttonState == ButtonState.Start)
                return;

			buttonStartStop.Enabled = false;

			videoSource.Stop();
			videoSource.WaitForStop();
			splitter.Stop();
			camView.Image = new Bitmap(Program.settings.VideoWidth, Program.settings.VideoHeight);

			// check for opened Serial Port implemented inside Gps Reciever class
			gps.Close();
			FpsDisplayer.Enabled = false;

			buttonStartStop.Text = resStart;
			buttonState = ButtonState.Start;
			
			buttonStartStop.Enabled = true;
		}

		private void StartStopRecording()
		{
			switch (buttonState)
			{
				case ButtonState.Start:
					StartRecording();
					break;

				case ButtonState.Stop:
					StopRecording();
					break;
			}
		}

		private void buttonStartStop_Click(object sender, EventArgs e)
		{
			StartStopRecording();
		}

		private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
            StopRecording();

            StreamWriter sr = File.CreateText(@"c:\shutting down.txt");
            sr.WriteLine(e.CloseReason.ToString());
            sr.Close();
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

		private void timerDebug_Tick(object sender, EventArgs e)
		{
			videoSource_NewFrame(this, null);
		}

		private void AutostartDelayer_Handler(object sender, EventArgs e)
		{
			VideoInitialization();
		}

		private void FpsDisplayer_Tick(object sender, EventArgs e)
		{
			lock (framesCountKeeper)
			{
				lastFps = totalFrames - lastFrames;
				lastFrames = totalFrames;
			}
		}

		private void timerWriter_Tick(object sender, EventArgs e)
		{
			lock (frameKeeper)
			{
				splitter.AddFrame(ref frame);

				if (Visible)
					camView.Image = frame;
			}
		}

		public enum FillMode
		{
			Normal,
			Full
		}

		private FillMode VideoWindowMode { get; set; }
		private const int SpaceToButtons = 26;

		private void MakeFullWindowVideo()
		{
			camView.Dock = DockStyle.Fill;
			VideoWindowMode = FillMode.Full;
		}

		private void MakeSmallSizedVideo()
		{
			camView.Dock = DockStyle.None;
			camView.Size = new Size(buttonSettings.Left - SpaceToButtons, buttonStartStop.Top - SpaceToButtons);
			camView.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			VideoWindowMode = FillMode.Normal;
		}

		private void camView_Click(object sender, EventArgs e)
		{
			if (VideoWindowMode == FillMode.Normal)
				MakeFullWindowVideo();
			else if (VideoWindowMode == FillMode.Full)
				MakeSmallSizedVideo();
		}
	}
}
