using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace CarDVR
{
	public partial class MainForm : Form
	{
		public enum FillMode
		{
			Normal,
			Full
		}

		private FillMode VideoWindowMode { get; set; }
		private const int SpaceToButtons = 26;
		private bool isFormActive = true;

		private void buttonMaximize_Click(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Normal)
			{
				buttonMaximize.Text = Resources.Normalize;
				this.WindowState = FormWindowState.Maximized;
			}
			else if (WindowState == FormWindowState.Maximized)
			{
				buttonMaximize.Text = Resources.Maximize;
				this.WindowState = FormWindowState.Normal;
			}
		}

		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Normal)
			{
				buttonMaximize.Text = Resources.Maximize;
			}
			else if (WindowState == FormWindowState.Maximized)
			{
				buttonMaximize.Text = Resources.Normalize;
			}
		}

		private void MainForm_Activated(object sender, EventArgs e)
		{
			isFormActive = true;
		}

		private void MainForm_Deactivate(object sender, EventArgs e)
		{
			isFormActive = false;
		}

		private void MakeFullWindowVideo()
		{
			camView.Dock = DockStyle.Fill;
			VideoWindowMode = FillMode.Full;
			statusBar.Visible = false;
		}

		private void MakeSmallSizedVideo()
		{
			camView.Dock = DockStyle.None;
			camView.Size = new Size(buttonSettings.Left - SpaceToButtons, buttonStartStop.Top - SpaceToButtons);
			camView.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			VideoWindowMode = FillMode.Normal;
			statusBar.Visible = true;
		}

		private void buttonMinimize_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}
	}
}
