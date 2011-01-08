using System;
using System.IO.Ports;

namespace CarDVR
{
	class GpsReciever
	{
		public GpsFormat gps = new NmeaImpl();
		public GpsState State { get; set; }

		private SerialPort com = new SerialPort();
		private bool IsInitialized = false;

		public GpsReciever()
		{
			com.DataReceived += new SerialDataReceivedEventHandler(com_DataReceived);
			State = GpsState.NotActive;
		}

		public void Initialize(string port, int baud)
		{
			IsInitialized = false;

			bool wasOpened = com.IsOpen;

			try
			{
				if (wasOpened) com.Close();

				com.PortName = port;
				com.BaudRate = baud;
				
				if (wasOpened) com.Open();

				State = GpsState.Active;
				IsInitialized = true;
			}
			catch (Exception) { }
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

				State = gps.FixTaken ? GpsState.NoSignal : GpsState.Active;
			}
		}
		#endregion

		#region Main SerialPort operations
		public bool Open()
		{
			State = GpsState.NotActive;

			if (!IsInitialized)
				return false;

			try
			{
				com.Open();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public void Close()
		{
			if (!com.IsOpen)
				return;

			try
			{
				com.Close();
			}
			catch (InvalidOperationException) { }
		}

		public bool IsOpened()
		{
			return com.IsOpen;
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
				return gps.NumberOfSatellites;
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
