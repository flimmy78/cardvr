using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace CarDVR
{
	public partial class MainForm : Form
	{
		const int maxFrames = 10;
		int index = maxFrames - 1;
		List<Bitmap> drawingFrames = new List<Bitmap>(maxFrames);
		object frameKeeper = new object();

		private void InitDrawingFrames()
		{
			drawingFrames.Clear();

			for (int t = 0; t < maxFrames; ++t)
				drawingFrames.Add(null);
		}

		private void PrepareFrameToDraw(Bitmap newFrame)
		{
			if (Program.settings.DontShowVideoWhenInactive && !isFormActive)
				return;

			lock (frameKeeper)
			{
				index++;
				index = index % maxFrames;

				Bitmap b = drawingFrames[index];

				while (b != null && b.Tag != null)
				{
					index++;
					index = index % maxFrames;
					b = drawingFrames[index];
				}

				if (b != null)
					b.Dispose();

				drawingFrames[index] = (Bitmap)newFrame.Clone();
			}
		}

		private void DrawFrameOnForm()
		{
			if (Program.settings.DontShowVideoWhenInactive && !isFormActive)
				return;

			lock (frameKeeper)
			{
				if (drawingFrames[index] == null)
					return;

				if (camView.Image != null)
					camView.Image.Tag = null;

				camView.Image = drawingFrames[index];
				camView.Image.Tag = 1;
			}
		}
	}
}
