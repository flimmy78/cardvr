using System;
using System.Collections.Generic;
using System.Text;

namespace Gps
{
	/// <summary>
	/// This interface create abstraction above different Gps Protocol formats
	/// </summary>
	public interface GpsStandard
	{
		void Initialize();
		void Parse(string line);

		bool FixTaken { get; }		
		string Coordinates { get; }
		int FixedSatellites { get; }
		string Speed { get; }
	}
}
