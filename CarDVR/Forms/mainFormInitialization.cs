using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Drawing;

namespace CarDVR
{
	public partial class MainForm : Form
	{
		Bitmap frame = null;
		object frameKeeper = new object();

		private void SetLocalization(string language)
		{
			CultureInfo ci = new CultureInfo(language == "Russian" ? "ru-RU" : "en-US");
			Thread.CurrentThread.CurrentUICulture = ci;
		}

		private void BeforeInitializeComponent()
		{
			Program.settings.Read();
			SetLocalization(Program.settings.Language);
			InitDynamicResources(Program.settings.Language);
			VideoWindowMode = FillMode.Normal;
			videoManager.NewFrame += videoManager_NewFrame;
		}

		private void AfterInitializeComponent()
		{
			Text = Application.ProductName + " v" + GetProgramVersion();

			IsWebCamAvaliable();

			if (Program.settings.StartWithFullWindowedVideo)
				MakeFullWindowVideo();

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
					string text = "On start delay";
					g.DrawString(text, framefont, Brushes.Black, message.Width / 2 - text.Length / 2 * framefont.Size, message.Height / 2);
				}
				camView.Image = message;

				// Protect pressing button "start"
				ButtonStartStopDisable();
			}
			else
				VideoInitialization();

			if (!Program.settings.StartMinimized)
				Show();
		}

		private string GetProgramVersion()
		{
			string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			return version.Substring(0, version.Length - 4);
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
	}
}
