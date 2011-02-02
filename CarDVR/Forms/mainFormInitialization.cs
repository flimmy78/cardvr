using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Drawing;
using AForge.Video;

namespace CarDVR
{
	public partial class MainForm : Form
	{
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
			Text = Application.ProductName + " v" + GetProgramVersion();

			IsWebCamAvaliable();

			if (Program.settings.AutostartRecording && Program.settings.DelayBeforeStart > 0)
			{
				autostartDelayer = new AutostartDelayer
									(
										Program.settings.DelayBeforeStart * 1000,
										AutostartDelayer_Handler
									);

				// TODO: make class BitmapDrawer
				Bitmap message = new Bitmap(Program.settings.VideoWidth, Program.settings.VideoHeight);

				using (Graphics g = Graphics.FromImage(message))
				{
					Font framefont = new Font("Arial", 18, FontStyle.Bold);
					g.DrawString
					(
						Resources.OnStartDelay, 
						framefont, 
						Brushes.Black,
						message.Width / 2 - Resources.OnStartDelay.Length / 2 * framefont.Size, 
						message.Height / 2
					);
				}
				camView.Image = message;

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

		private void CommonInitialization()
		{
			if (Program.settings.HideMouseCursor)
				Cursor.Hide();

			SetColor(this, Color.FromArgb(Program.settings.InterfaceForeColor), Color.FromArgb(Program.settings.InterfaceBackgroundColor));
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
