using System;
using System.IO.Ports;

namespace Gps
{
	/// <summary>
	/// Receives info from GPS device. Only NMEA format implemented.
	/// </summary>
	public class GpsReceiver
	{
		public GpsStandard gps = new NmeaImpl();
		public GpsState State { get; set; }

		private SerialPort com = new SerialPort();
		private bool IsInitialized = false;

		public GpsReceiver()
		{
			com.DataReceived += new SerialDataReceivedEventHandler(com_DataReceived);
			State = GpsState.NotActive;
		}

		public void Initialize(string port, int baud)
		{
			try
			{
				IsInitialized = false;

				gps.Initialize();
				State = GpsState.NotActive;

				bool wasOpened = com.IsOpen;

				if (wasOpened) com.Close();

				com.PortName = port;
				com.BaudRate = baud;

				if (wasOpened) com.Open();

				IsInitialized = true;
			}
			catch (Exception e) 
			{
				throw new Exception(string.Format("Can't open serial port '{0}'. Description: ", port, e.Message));
			}
		}

		#region SerialPort data reception
		
		private string buff = string.Empty;
		private static readonly string rn = "\r\n";

		void com_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			buff += com.ReadExisting();

			while (buff.Contains(rn))
			{
				int rnPos = buff.IndexOf(rn);

				string line = buff.Substring(0, rnPos);
				buff = buff.Remove(0, rnPos + rn.Length);

				gps.Parse(line);

				State = gps.FixTaken ? GpsState.Active : GpsState.NoSignal;
			}
		}
		#endregion

		#region Main SerialPort operations
		public void Open()
		{
			State = GpsState.NotActive;

			if (!IsInitialized)
				throw new TypeInitializationException(this.GetType().Name, new Exception("Not initialized"));

			com.Open();
		}

		public void Close()
		{
			try
			{
				if (com.IsOpen)
				{
					com.Close();
				}
			}
			catch { /* skip all errors */ }
		}

		public bool IsOpened()
		{
			try
			{
				return com.IsOpen;
			}
			catch
			{
				return false;
			}
		}
		#endregion

		#region GpsProtocol interface implementation
		public string Coordinates
		{
			get
			{
				return State == GpsState.Active ? gps.Coordinates : string.Empty;
			}
		}

		public int NumberOfSatellites
		{
			get
			{
				return gps.FixedSatellites;
			}
		}

		public string Speed
		{
			get
			{
				return gps.Speed;
			}
		}
		#endregion
	}
}
