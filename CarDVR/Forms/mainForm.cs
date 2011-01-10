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
		private static bool VideosourceInitialized = false;
		private AutostartDelayer autostartDelayer;
		private static ResourceManager resources;

		private static string resSpeed;
		private static string resKmh;
		private static string resSatellites;
		private static string resNoGpsSignal;
		private static string resGpsNotConnected;		
		
		VideoCaptureDevice videoSource = null;
		GpsReceiver gps;
		VideoSplitter splitter;
		Bitmap frame;
		object frameKeeper = new object();

		public MainForm()
		{
			Program.settings.Read();

			CultureInfo ci = new CultureInfo(Program.settings.Language.Equals("Russian") ? "ru-RU" : "en-US");
			Thread.CurrentThread.CurrentUICulture = ci;

			VideoWindowMode = FillMode.Normal;

			InitializeComponent();

			IsWebCamAvaliable();

			resources = new ResourceManager("CarDVR.mainForm", GetType().Assembly);

			if (Program.settings.StartWithFullWindowedVideo)
				MakeFullWindowVideo();

			if (Program.settings.AutostartRecording && Program.settings.DelayBeforeStart > 0)
				autostartDelayer = new AutostartDelayer(
											Program.settings.DelayBeforeStart * 1000, 
											AutostartDelayer_Handler
											);
			else
				GlobalInitialization();
		}

		private void InitVideoSource()
		{
			VideosourceInitialized = false;

			bool running = false;
			if (videoSource != null && videoSource.IsRunning)
			{
				running = true;
				StartStopRecording();
			}

#if DEBUG
			running = splitter.IsRunning && (Program.settings.GetVideoSize() != splitter.VideoSize);
#endif

			if (videoSource != null)
				videoSource.NewFrame -= videoSource_NewFrame;

			videoSource = new VideoCaptureDevice(Program.settings.VideoSourceId);
			videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
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

			IsWebCamAvaliable();

			// init static resources
			resSpeed = resources.GetString("Speed:");
			resKmh = resources.GetString("km/h");
			resSatellites = resources.GetString("Satellites:"); ;
			resNoGpsSignal = resources.GetString("No GPS signal"); ;
			resGpsNotConnected = resources.GetString("GPS not connected");

			if (running)
				StartStopRecording();

			VideosourceInitialized = true;
		}


		private void GlobalInitialization()
		{
			// Create avi-splitter. It will be initialized in InitVideoSource()
			splitter = new VideoSplitter();
			gps = new GpsReceiver();

			InitializeGpsIfEnabled(Program.settings.GpsSerialPort, Program.settings.SerialPortBaudRate);

			// create first video source
			InitVideoSource();

			string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			Text += " v" + version.Substring(0, version.Length - 4);

			if (!Program.settings.StartMinimized)
				Show();

			buttonState = ButtonState.Start;

			if (Program.settings.AutostartRecording
#if !DEBUG
 && !string.IsNullOrEmpty(Program.settings.VideoSource)
#endif
)
				StartStopRecording();

		}

		int lastFrames = 0, totalFrames = 0, lastFps = 0;
		object framesKeeper = new object();

		private string MakeFrameString()
		{
			string result = DateTime.Now.ToString() + " ";

			// Do not write anything if GPS disabled in settings
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

			lock (framesKeeper)
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
#if DEBUG
			frame = (Bitmap)Bitmap.FromFile("../../Resources/1.jpg");
#else
				frame = (Bitmap)eventArgs.Frame.Clone();
#endif


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

				// if settings not applied yet
				if (!VideosourceInitialized || frame.Size != Program.settings.GetVideoSize())
					return;

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

#if DEBUG
			WebCamPresent = true;
#else
			WebCamPresent = !string.IsNullOrEmpty(Program.settings.VideoSource);
#endif

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
				if (!Program.settings.GpsEnabled)
					gps.Close();
				else
					InitializeGpsIfEnabled(Program.settings.GpsSerialPort, Program.settings.SerialPortBaudRate);
			}
		}

		private void InitializeGpsIfEnabled(string port, int baud)
		{
			try
			{
				if (Program.settings.GpsEnabled)
					gps.Initialize(port, baud);
			}
			catch (Exception e)
			{
				Reporter.NonSeriousError(e.Message);
			}
		}

		private void StartRecording()
		{
			// Update settings, may be web cam became not avaliable
			Program.settings.Read();
			IsWebCamAvaliable();

			InitializeGpsIfEnabled(Program.settings.GpsSerialPort, Program.settings.SerialPortBaudRate);

			if (Program.settings.GpsEnabled)
			{
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

			splitter.Start();
#if DEBUG
					timerDebug.Start();
#else
			videoSource.Start();
#endif
			FpsDisplayer.Enabled = true;
		}

		private void StopRecording()
		{
			videoSource.Stop();
			videoSource.WaitForStop();
			splitter.Stop();
			camView.Image = new Bitmap(Program.settings.VideoWidth, Program.settings.VideoHeight);
			timerDebug.Stop();

			// check for opened Serial Port implemented inside Gps Reciever class
			gps.Close();
			FpsDisplayer.Enabled = false;
		}

		private void StartStopRecording()
		{
			buttonStartStop.Enabled = false;

			switch (buttonState)
			{
				case ButtonState.Start:
					StartRecording();
					break;

				case ButtonState.Stop:
					StopRecording();
					break;
			}

			buttonStartStop.Text = buttonState == ButtonState.Start ? resources.GetString("Stop") : resources.GetString("buttonStartStop.Text");
			buttonState = buttonState == ButtonState.Start ? ButtonState.Stop : ButtonState.Start;
			buttonStartStop.Enabled = true;
		}

		private void buttonStartStop_Click(object sender, EventArgs e)
		{
			StartStopRecording();
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

		private void timerDebug_Tick(object sender, EventArgs e)
		{
			videoSource_NewFrame(this, null);
		}

		private void AutostartDelayer_Handler(object sender, EventArgs e)
		{
			GlobalInitialization();
		}

		private void FpsDisplayer_Tick(object sender, EventArgs e)
		{
			lock (framesKeeper)
			{
				lastFps = totalFrames - lastFrames;
				lastFrames = totalFrames;
			}
		}

		private void timerWriter_Tick(object sender, EventArgs e)
		{
			if (!VideosourceInitialized)
				return;

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
