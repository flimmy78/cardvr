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
		private static RecordingState recordingState = RecordingState.Stopped;
		private AutostartDelayer autostartDelayer;

		static GpsReceiver gps = new GpsReceiver();
		public VideoManager videoManager = new VideoManager(gps);

		public MainForm()
		{
			BeforeInitializeComponent();

			InitializeComponent();

			AfterInitializeComponent();

			for (int t = 0; t < maxG; ++t)
				g.Add(null);
		}

		private void SafeVideoManagerInitialize()
		{
			try
			{
				videoManager.Initialize();
			}
			catch (NoWebcamException)
			{
				Reporter.SeriousError("Current webcam is not avalizble.");
			}
			catch (Exception e)
			{
				Reporter.SeriousError(e.Message);
			}
		}
		
		private void VideoInitialization()
		{
			try
			{
				videoManager.Initialize();

				if (Program.settings.AutostartRecording && !string.IsNullOrEmpty(Program.settings.VideoSource))
					StartStopRecording();
			}
			catch
			{
			}
		}

		private bool IsWebCamAvaliable()
		{
			if (!string.IsNullOrEmpty(Program.settings.VideoSource) && 
				videoManager.SureThatWebcamExists(Program.settings.VideoSourceId))
			{
				ButtonStartStopEnable();
				HideNoVideosourceWarning();
				return true;
			}

			ButtonStartStopDisable();
			ShowNoVideosourceWarning();

			return false;
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
					bool isRecording = videoManager.IsRecording();

					if (isRecording)
						StopRecording();

					try
					{
						videoManager.Initialize();

						if (isRecording)
							StartRecording();
					}
					catch
					{
						Reporter.SeriousError("Current webcam is not avalizble.");
					}
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
			if (recordingState == RecordingState.Started)
				return;

			ButtonStartStopDisable();

			// Update settings, may be web cam became not avaliable
			Program.settings.Read();
			IsWebCamAvaliable();

			InitializeGps();
			StartGpsReceiver();

			videoManager.Start();
			videoDrawer.Enabled = true;
			
			buttonStartStop.Text = resStop;
			recordingState = RecordingState.Started;

			ButtonStartStopEnable();
		}

		public void StopRecording()
		{
			if (recordingState == RecordingState.Stopped)
				return;

			ButtonStartStopDisable();

			gps.Close();
			videoManager.Stop();
			videoDrawer.Enabled = false;

			// TODO: make class ImageDrawer (use empty)
			camView.Image = new Bitmap(Program.settings.VideoWidth, Program.settings.VideoHeight);

			buttonStartStop.Text = resStart;
			recordingState = RecordingState.Stopped;

			ButtonStartStopEnable();
		}

		private void StartStopRecording()
		{
			switch (recordingState)
			{
				case RecordingState.Stopped:
					StartRecording();
					break;

				case RecordingState.Started:
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
			if (videoManager.IsRecording())
			{
				StopRecording();
				videoManager.Close();
			}
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

		private void AutostartDelayer_Handler(object sender, EventArgs e)
		{
			// TODO: make class BitmapDrawer
			camView.Image = new Bitmap(Program.settings.VideoWidth, Program.settings.VideoHeight);

			ButtonStartStopEnable();
			VideoInitialization();
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


		const int maxG = 10;
		int cnt = maxG-1;
		List<Bitmap> g = new List<Bitmap>(maxG);
	   
		void videoManager_NewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			if (!Visible)
				return;

			lock (frameKeeper)
			{
				cnt++;
				cnt = cnt % maxG;

				Bitmap b = g[cnt];

				while (b != null && b.Tag != null)
				{
					cnt++;
					cnt = cnt % maxG;
					b = g[cnt];
				}

				if (b != null)
					b.Dispose();

				g[cnt] = (Bitmap)eventArgs.Frame.Clone();
			}
		}

		private void videoDrawer_Tick(object sender, EventArgs e)
		{
			if (!Visible)
				return;

			lock (frameKeeper)
			{
				if (g[cnt] == null)
					return;

				if (camView.Image != null)
					camView.Image.Tag = null;

				camView.Image = g[cnt];
				camView.Image.Tag = 1;
			}
		}

		private void buttonMaximize_Click(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Normal)
			{
				buttonMaximize.Text = resNormalize;
				this.WindowState = FormWindowState.Maximized;
			}
			else if (WindowState == FormWindowState.Maximized)
			{
				buttonMaximize.Text = resMaximize;
				this.WindowState = FormWindowState.Normal;
			}
		}

		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Normal)
			{
				buttonMaximize.Text = resMaximize;
			}
			else if (WindowState == FormWindowState.Maximized)
			{
				buttonMaximize.Text = resNormalize;
			}
		}
	}
}
