using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CarDVR
{
	public partial class MainForm : Form
	{
		public static string resSpeed;
		public static string resKmh;
		public static string resSatellites;
		public static string resNoGpsSignal;
		public static string resGpsNotConnected;
		public static string resStop;
		public static string resStart;

		private void InitRussianResources()
		{
			resSpeed = "Скорость:";
			resKmh = "км/ч";
			resSatellites = "Спутников:";
			resNoGpsSignal = "Нет сигнала GPS";
			resGpsNotConnected = "GPS не подключен";
			resStop = "Стоп";
			resStart = "Старт";
		}

		private void InitEnglishResources()
		{
			resSpeed = "Speed:";
			resKmh = "km/h";
			resSatellites = "Satellites:";
			resNoGpsSignal = "No GPS signal";
			resGpsNotConnected = "GPS not connected";
			resStop = "Stop";
			resStart = "Start";
		}			 
			
		private void InitDynamicResources(string language)
		{
			if (language == "English")
				InitEnglishResources();
			else if (language == "Russian")
				InitRussianResources();
		}
	}
}
