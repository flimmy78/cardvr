using System;
using System.Collections.Generic;
using System.Text;

namespace CarDVR
{
	/// <summary>
	/// This class implements IGpsFormat interface and declares NMEA Gps Protocol
	/// </summary>
	class NmeaImpl : GpsFormat
	{
		private string latitude = string.Empty;
		private string longitude = string.Empty;

		private int fixTaken = 0;
		private int numberOfSatellites = 0;

		public NmeaSpeed speed = new NmeaSpeed();

		public bool FixTaken
		{
			get
			{
				return fixTaken == 1;
			}
		}

		public void Parse(string line)
		{
			string[] parameters = line.Split(',');

			if (parameters.Length == 0)
				return;

			if (parameters[0] == "$GPRMC")
				ParseRMC(ref parameters);
			else if (parameters[0] == "$GPGGA")
				ParseGGA(ref parameters);
		}

		void ParseGGA(ref string[] parameters)
		{
			if (parameters.Length <= 7)
				return;

			latitude = parameters[3] + " ";
			latitude += parameters[2].Insert(2, " ");

			longitude = parameters[5] + " ";
			longitude += parameters[4].Insert(3, " ");
			if (longitude.Contains(" 0"))
				longitude = longitude.Remove(2, 1);

			if (!int.TryParse(parameters[6], out fixTaken))
				fixTaken = 0;

			if (!int.TryParse(parameters[7], out numberOfSatellites))
				numberOfSatellites = 0;
		}

		void ParseRMC(ref string[] parameters)
		{
			string velocity = string.Empty;

			if (parameters.Length > 7)
				velocity = parameters[7];

			speed.Convert(velocity);
		}

		public string Coordinates
		{
			get
			{
				return latitude + " " + longitude;
			}
		}

		public int NumberOfSatellites
		{
			get
			{
				return numberOfSatellites;
			}
		}

		public string Speed
		{
			get
			{
				return speed.ToString();
			}
		}
	}
}
