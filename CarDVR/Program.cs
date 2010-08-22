using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace CarDVR
{
    static class Program
    {
		static MainForm mainform;
		public static SettingsImpl settings = new SettingsImpl();

		class MyAppContext : ApplicationContext
		{
			public MyAppContext()
			{
				mainform = new MainForm();
			}
		}

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
			Application.Run(new MyAppContext());
        }
    }
}
