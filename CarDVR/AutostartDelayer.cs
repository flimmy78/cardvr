using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CarDVR
{
	class AutoStartDelayer
	{
		Timer timer = new Timer();
		EventHandler callback_;

		public AutoStartDelayer(int pause, EventHandler callback)
		{
			callback_ = callback;
			timer.Interval = pause;
			timer.Enabled = true;
			timer.Tick += new EventHandler(timer_Tick);
			timer.Tick += new EventHandler(callback);
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			timer.Enabled = false;
			timer.Tick -= timer_Tick;
			timer.Tick -= callback_;
		}
	}
}
