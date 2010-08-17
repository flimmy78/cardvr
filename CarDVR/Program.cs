using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MyAppContext());
        }
    }
}
