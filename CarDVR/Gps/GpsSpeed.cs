using System;
using System.Globalization;

namespace CarDVR
{
	/// <summary>
	/// This Class converts NMEA knots to km/h
	/// </summary>
	public class NmeaSpeed
	{
		private static readonly double KMpHPerKnot = 1.852;
		public bool ConvertedOk { get; private set; }
		public int Value { get; private set; }

		public void Convert(string velocity)
		{
			if (string.IsNullOrEmpty(velocity))
			{
				MakeResultEmpty();
				return;
			}

			try
			{
				Value = (int)(double.Parse(velocity, CultureInfo.InvariantCulture) * KMpHPerKnot);
				ConvertedOk = true;
			}
			catch (Exception)
			{
				MakeResultEmpty();
			}
		}

		public override string ToString()
		{
			return ConvertedOk ? Value.ToString() : "conversion error";
		}

		private void MakeResultEmpty()
		{
			Value = 0;
			ConvertedOk = false;
		}
	}
}
