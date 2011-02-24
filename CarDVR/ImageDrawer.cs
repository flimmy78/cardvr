using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CarDVR
{
	class ImageDrawer
	{
		static public Bitmap GetEmptyImage()
		{
			return new Bitmap(1, 1);
		}

		static public Bitmap CreateImage(int width, int height, string text)
		{
			Bitmap image = new Bitmap(width, height);

			using (Graphics g = Graphics.FromImage(image))
			{
				Font framefont = new Font("Arial", 18, FontStyle.Bold);
				g.DrawString
				(
					text,
					framefont,
					Brushes.Black,
					image.Width / 2 - text.Length / 2 * framefont.Size,
					image.Height / 2
				);
			}

			return image;
		}
	}
}
