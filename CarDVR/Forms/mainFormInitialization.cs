using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Resources;

namespace CarDVR
{
	public partial class MainForm : Form
	{
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
			}
			else
				VideoInitialization();

			if (!Program.settings.StartMinimized)
				Show();

			buttonState = ButtonState.Start;
		}
		private string GetProgramVersion()
		{
			string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			return version.Substring(0, version.Length - 4);
		}
	}
}
