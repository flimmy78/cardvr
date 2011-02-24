using System;
using System.Globalization;

namespace Gps
{
	/// <summary>
	/// This Class converts NMEA knots to km/h
	/// </summary>
	public class NmeaSpeedCalculator
	{
		private static readonly double KMpHPerKnot = 1.852;

		public static int Convert(string velocity)
		{
			if (string.IsNullOrEmpty(velocity))
				throw new ArgumentException();

			int value = 0;

			try
			{
				value = (int)(double.Parse(velocity, CultureInfo.InvariantCulture) * KMpHPerKnot);
			}
			catch
			{
				throw new ArgumentException(); 
			}

			return value;
		}
	}
}
