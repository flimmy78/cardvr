﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Timers;

namespace CarDVR
{
    public class SpeedEventArgs : EventArgs
    {
        private int _speed;
        public SpeedEventArgs(int speed)
        {
            _speed = speed;
        }
        public int Speed
        {
            get
            {
                return _speed;
            }
        }
    }
    public enum GpsState
    {
        NotActive,
        Active,
        NoSignal
    }

    class GpsReciever
    {
        private static readonly double KMpHPerKnot = 1.852;
        private static readonly string rn = "\r\n";

        private SerialPort com = null;
        private string buff = string.Empty;
        private bool IsInitialized = false;
        private GpsState gpsState = GpsState.NotActive;
        private DateTime lastSpeedUpdate = DateTime.Now;
        private string latitude = string.Empty;
        private string longitude = string.Empty;
        private int speed = 0;

        private int fixTaken = 0;
        private int numberOfSattelites = 0;

        public GpsState GpsState
        {
            get
            {
                return gpsState;
            }
        }
        
        public GpsReciever()
        {
            com = new SerialPort();
            com.DataReceived += new SerialDataReceivedEventHandler(com_DataReceived);
        }

        public void Initialize(string comport, int baud)
        {
            if (com.IsOpen)
                return;

            com.PortName = comport;
            com.BaudRate = baud;
            gpsState = GpsState.Active;
            IsInitialized = true;
        }

        ~GpsReciever()
        {
            try
            {
                if (com.IsOpen)
                    com.Close();
            }
            catch (Exception) { }

            com.DataReceived -= com_DataReceived;
            com.Dispose();
        }

        string ParseSentence(string sentenceName, int paramNum)
        {
            if (!buff.Contains(sentenceName))
                return string.Empty;
            
            buff = buff.Substring(buff.IndexOf(sentenceName));

            if (!buff.Contains(rn))
                return string.Empty;

            int eol = buff.IndexOf(rn);
            string sents = buff.Substring(0, eol);
            buff.Remove(0, eol);

            String[] lines = sents.Split(',');

            return lines.Length > paramNum ? lines[paramNum] : string.Empty;
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

            string outstring = latitude + " " + longitude;

            if (!int.TryParse(parameters[6], out fixTaken))
                fixTaken = 0;

            if (!int.TryParse(parameters[7], out numberOfSattelites))
                numberOfSattelites = 0;

            gpsState = fixTaken == 0 ? CarDVR.GpsState.NoSignal : CarDVR.GpsState.Active;
        }

        void ParseRMC(ref string[] parameters)
        {
            if (parameters.Length <= 7)
                return;

            string velocity = parameters[7];

            if (!String.IsNullOrEmpty(velocity))
            {
                lastSpeedUpdate = DateTime.Now;

                try
                {
                    speed = Convert.ToInt32(Convert.ToDouble(velocity.Replace('.', ',')) * KMpHPerKnot);
                }
                catch (Exception) { }
            }
        }

        void com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            buff += com.ReadExisting();

            while (buff.Contains(rn))
            {
                int rnPos = buff.IndexOf(rn);

                string line = buff.Substring(0, rnPos);
                buff = buff.Remove(0, rnPos + rn.Length);

                string[] parameters = line.Split(',');

                if (parameters.Length == 0)
                    continue;

                if (parameters[0] == "$GPRMC")
                    ParseRMC(ref parameters);
                else if (parameters[0] == "$GPGGA")
                    ParseGGA(ref parameters);
            }

        }

        public bool Open()
        {
            gpsState = CarDVR.GpsState.NotActive;

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
            catch (Exception) { }
        }

        public string Coordinates
        {
            get
            {
                if (gpsState != GpsState.Active)
                    return string.Empty;

                return latitude + " " + longitude;
            }
        }

        public int NumberOfSattelites
        {
            get
            {
                return numberOfSattelites;
            }
        }

        public int Speed
        {
            get
            {
                return speed;
            }
        }

        public bool IsOpened()
        {
            return com.IsOpen;
        }
    }
}
