using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CarDVR
{
	class AutoStartDelayer
	{
		private Timer timer = new Timer();

		public AutoStartDelayer(int pause, EventHandler callback)
		{
			timer.Interval = pause;
			timer.Enabled = true;
			timer.Tick += new EventHandler(timer_Tick);
			timer.Tick += new EventHandler(callback);
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			timer.Enabled = false;
			timer.Tick -= timer_Tick;
		}
	}
}
