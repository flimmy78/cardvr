using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AForge.Video.VFW;
using System.Reflection;
using Microsoft.Win32;
using System.Windows.Forms;

namespace CarDVR
{
	class SerialPortBaudRates
	{
		public static int[] values = { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200, 128000, 256000 };
	}

    class NoWebcamException : Exception
    {
    }

    public class Reporter
    {
   	    static public void Error(string text)
	    {
		    MessageBox.Show(text, "Error", MessageBoxButtons.OK);
	    }
    }

	enum ButtonState
	{
		Start,
		Stop
	}

	class AutorunHelper
	{
		public static readonly string AUTORUN_KEY = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
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
			catch (Exception)
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
			catch (Exception)
			{
				Program.settings.StartWithWindows = false;
				Program.settings.Save();
			}

			key.Close();
		}
	}

}
