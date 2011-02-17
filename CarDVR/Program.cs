using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace CarDVR
{
	static class Program
	{
		public static MainForm mainform;
		public static SettingsImpl settings = new SettingsImpl();

		[STAThread]
		static void Main()
		{
			int processCount = 0;

			foreach (Process p in Process.GetProcesses())
				if (p.ProcessName == Process.GetCurrentProcess().ProcessName)
				{
					if (++processCount == 2)
						return;
				}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			mainform = new MainForm();
			Application.Run(mainform);
		}
	}
}

