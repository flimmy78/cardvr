using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security;
using System.Security.Permissions;
using System.Security.AccessControl;
using System.IO;
using System.Security.Principal;

namespace CarDVR
{
	class SerialPortBaudRates
	{
		public static int[] values = { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200, 128000, 256000 };
	}

	class NoWebcamException : Exception
	{
	}

	class WebcamPropertiesException : Exception
	{
	}

	// Simple error reporter / error holder
	// 
	// Serious errors always be shown in MessageBox,
	// non-serious errors will be shown on video frame and be alerted by specific sound.
	//
	// There's no synchronization, because Timer works in current thread
	public class Reporter
	{
		public static void NonSeriousError(string text)
		{
			InitializeErrorCleaner();
			longLivingNonSeriousError = text;
			errorCleaner.Enabled = true;
		}

		public static string GetNonSeriousError()
		{
			return longLivingNonSeriousError;
		}

		public static void SeriousError(string text)
		{
			MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private static void InitializeErrorCleaner()
		{
			if (errorCleaner == null)
			{
				errorCleaner = new Timer();
				errorCleaner.Interval = 5000;
				errorCleaner.Enabled = false;
				errorCleaner.Tick += new EventHandler(errorCleaner_Tick);
			}
		}

		private static void errorCleaner_Tick(object sender, EventArgs e)
		{
			errorCleaner.Enabled = false;
			longLivingNonSeriousError = string.Empty;
		}

		static string longLivingNonSeriousError = string.Empty;
		static Timer errorCleaner = null;
		static object errorCleanerHolder = new object();
	}

	enum RecordingState
	{
		Stopped,
		Started
	}

	class DirectoryWriteChecker
	{
		public static bool Process(string dir)
		{
			string filename = dir + "/test.avi";

			try
			{
				using (FileStream fs = File.Create(filename))
				{
					fs.Close();
					File.Delete(filename);
				}
			}
			catch
			{
				return false;
			}

			return true;
		}
	}

	class AutorunHelper
	{
		private static readonly string AUTORUN_KEY = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";

		public static void SetEnabled(bool enabled)
		{
			if (enabled)
				EnableAutorun();
			else
				DisableAutorun();
		}

		public static void EnableAutorun()
		{
			Assembly asm = Assembly.GetExecutingAssembly();

			string exepath = asm.Location;
			string CarDrv = asm.GetName().Name;

			RegistryKey key = Registry.CurrentUser;

			try
			{
				key = Registry.CurrentUser.OpenSubKey(AUTORUN_KEY, true);
				key.SetValue(CarDrv, exepath, RegistryValueKind.String);
			}
			catch
			{
				Program.settings.StartWithWindows = false;
				Program.settings.Save();
			}

			key.Close();
		}

		public static void DisableAutorun()
		{
			RegistryKey key = Registry.CurrentUser;
			string CarDvr = Assembly.GetExecutingAssembly().GetName().Name;

			try
			{
				key = Registry.CurrentUser.OpenSubKey(AUTORUN_KEY, true);
				key.DeleteValue(CarDvr);
			}
			catch
			{
				Program.settings.StartWithWindows = false;
				Program.settings.Save();
			}

			key.Close();
		}
	}
}
