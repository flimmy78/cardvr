using System;
using System.Collections.Generic;
using System.Text;

namespace CarDVR
{
	/// <summary>
	/// This interface create abstraction above different Gps Protocol formats
	/// </summary>
	interface GpsFormat
	{
		bool FixTaken { get; }
		void Parse(string line);
		string Coordinates { get; }
		int NumberOfSatellites { get; }
		string Speed { get; }
	}
}
