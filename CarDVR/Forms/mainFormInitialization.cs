using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Drawing;
using AForge.Video;

using CarDvrPipes;

namespace CarDVR
{
	public partial class MainForm : Form
	{
        PipesClient pipe = null;

		public MainForm()
		{
			BeforeInitializeComponent();

			InitializeComponent();

			AfterInitializeComponent();

            pipe = new PipesClient();
            pipe.Start();
		}

		private void BeforeInitializeComponent()
		{
			Program.settings.Read();
			SetLocalization(Program.settings.Language);
			Resources.InitDynamicResources(Program.settings.Language);
			VideoWindowMode = FillMode.Normal;
			videoManager.NewFrame += videoManager_NewFrame;
		}

		private void AfterInitializeComponent()
		{
			Text = string.Format("{0} v{1}", Application.ProductName, GetProgramVersion());

			IsWebCamAvaliable();

			if (Program.settings.AutostartRecording && Program.settings.DelayBeforeStart > 0)
			{
				autoStartDelayer = new AutoStartDelayer
				(
					Program.settings.DelayBeforeStart * 1000,
					AutostartDelayer_Handler
				);

				SetStatusBarStatus(StatusState.WaitingBeforeStart, Resources.WaitingBeforeStart);

				camView.Image = ImageDrawer.CreateImage(800, 600, Resources.OnStartDelay);

				ButtonStartStopDisable();
			}
			else
				VideoInitialization();

			if (Program.settings.StartMinimized)
				this.WindowState = FormWindowState.Minimized;

			InitDrawingFrames();

			if (Program.settings.StartWithFullWindowedVideo)
				MakeFullWindowVideo();

			CommonInitialization();
		}

		private bool IsWebCamAvaliable()
		{
			if
			(
				!string.IsNullOrEmpty(Program.settings.VideoSource) &&
				videoManager.SureThatWebcamExists(Program.settings.VideoSourceId)
			)
			{
				ButtonStartStopEnable();
				HideNoVideosourceWarning();
				return true;
			}

			ButtonStartStopDisable();
			ShowNoVideosourceWarning();
			return false;
		}

		private void CommonInitialization()
		{
			if (Program.settings.HideMouseCursor)
				Cursor.Hide();

			FormColorSetter.Do
			(
				this, 
				Color.FromArgb(Program.settings.InterfaceForeColor), 
				Color.FromArgb(Program.settings.InterfaceBackgroundColor)
			);
		}
	
		private void SetLocalization(string language)
		{
			CultureInfo ci = new CultureInfo(language == "Russian" ? "ru-RU" : "en-US");
			Thread.CurrentThread.CurrentUICulture = ci;
		}

		private string GetProgramVersion()
		{
			string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			
			while (version.Substring(version.Length - 2, 2) == ".0")
				version = version.Remove(version.Length - 2, 2);

			return version;
		}

		private void ButtonStartStopEnable()
		{
			buttonStartStop.Enabled = true;
		}

		private void ButtonStartStopDisable()
		{
			buttonStartStop.Enabled = false;
		}

		private void HideNoVideosourceWarning()
		{
			labelNoVideoSource.Visible = false;
		}

		private void ShowNoVideosourceWarning()
		{
			labelNoVideoSource.Visible = true;
		}

		private void SafeVideoManagerInitialize()
		{
			try
			{
				videoManager.Initialize();
			}
			catch (NoWebcamException)
			{
				Reporter.SeriousError(Resources.CurrentWebCamNotAvaliable);
			}
			catch (Exception e)
			{
				Reporter.SeriousError(e.Message);
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
				Reporter.UnseriousError(e.Message);
			}
		}

		private void VideoInitialization()
		{
			try
			{
				videoManager.Initialize();

				if (Program.settings.AutostartRecording && !string.IsNullOrEmpty(Program.settings.VideoSource))
					StartRecording();
			}
			catch { }
		}

		void videoManager_NewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			PrepareFrameToDraw(eventArgs.Frame);
		}
	}
}
