using System;
using System.Collections.Generic;
using System.Text;

namespace Gps
{
	/// <summary>
	/// This class implements GpsFormat interface and declares NMEA Gps Protocol
	/// </summary>
	class NmeaImpl : GpsStandard
	{
		private string latitude = string.Empty;
		private string longitude = string.Empty;
		private string speed = string.Empty;

		private int fixTaken = 0;
		private int fixedSatellites = 0;

		private string[] parameters;

		enum GPGGA
		{
			Name = 0,
			LatitudeValue = 2,
			Latitude = 3,
			LongitudeValue = 4,
			Longitude = 5,
			FixTaken = 6,
			FixedSattelites = 7,
			MaximalRequired = 8,
		}

		enum GPRMC
		{
			Name = 0,
			Velocity = 7,
			MaximalRequired = 8,
		}

		string GetParameter(GPGGA param)
		{
			return parameters[(int)param];
		}

		string GetParameter(GPRMC param)
		{
			return parameters[(int)param];
		}

		bool IsGpggaCommand()
		{
			return parameters[(int)GPGGA.Name] == "$GGGA";
		}

		bool IsGprmcCommand()
		{
			return parameters[(int)GPRMC.Name] == "$GPRMC";
		}

		public void Parse(string line)
		{
			parameters = line.Split(',');

			if (parameters.Length == 0)
				return;

			if (IsGprmcCommand()) 
				ParseRMC();
			else if (IsGpggaCommand()) 
				ParseGGA();
		}

		void ParseGGA()
		{
			if (parameters.Length+1 < (int)GPGGA.MaximalRequired)
				return;

			if (!int.TryParse(GetParameter(GPGGA.FixTaken), out fixTaken))
				fixTaken = 0;

			if (!int.TryParse(GetParameter(GPGGA.FixedSattelites), out fixedSatellites))
				fixedSatellites = 0;

			if (fixTaken == 1)
			{
				latitude = GetParameter(GPGGA.Latitude) + " ";
				string latitudeValue = GetParameter(GPGGA.LatitudeValue);

				if (!string.IsNullOrEmpty(latitudeValue))
					latitude += latitudeValue.Insert(2, " ");

				longitude = GetParameter(GPGGA.Longitude) + " ";
				string longitudeValue = GetParameter(GPGGA.LongitudeValue);

				if (!string.IsNullOrEmpty(longitudeValue))
					longitude += longitudeValue.Insert(3, " ");

				if (longitude.Contains(" 0"))
					longitude = longitude.Remove(2, 1);
			}
		}

		void ParseRMC()
		{
			if (parameters.Length + 1 < (int)GPRMC.MaximalRequired)
			{
				speed = string.Empty;
				return;
			}

			string velocity = GetParameter(GPRMC.Velocity);

			if (string.IsNullOrEmpty(velocity))
			{
				speed = string.Empty;
				return;
			}

			try
			{
				speed = NmeaSpeedCalculator.Convert(velocity).ToString();
			}
			catch
			{
				speed = string.Format("speed convertation error (velocity={0})", velocity);
			}
		}

		public bool FixTaken
		{
			get
			{
				return fixTaken == 1;
			}
		}

		public string Coordinates
		{
			get
			{
				return latitude + " " + longitude;
			}
		}

		public int FixedSatellites
		{
			get
			{
				return fixedSatellites;
			}
		}

		public string Speed
		{
			get
			{
				return speed;
			}
		}

		public void Initialize()
		{
			fixTaken = 0;
			speed = string.Empty;
			latitude = string.Empty;
			longitude = string.Empty;
		}
	}
}
