﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CarDVR
{
	public class Resources
	{
		public static string Speed;
		public static string Kmh;
		public static string Satellites;
		public static string NoGpsSignal;
		public static string GpsNotConnected;
		public static string Stop;
		public static string Start;
		public static string Maximize;
		public static string Normalize;
		public static string OnStartDelay;
		public static string CurrentWebCamNotAvaliable;
		public static string CantOpenGpsOnPort;

		static private void InitRussianResources()
		{
			Speed = "Скорость:";
			Kmh = "км/ч";
			Satellites = "Спутников:";
			NoGpsSignal = "Нет сигнала GPS";
			GpsNotConnected = "GPS не подключен";
			Stop = "Стоп";
			Start = "Старт";
			Maximize = "Развернуть";
			Normalize = "Вернуть";
			OnStartDelay = "Задержка запуска";
			CurrentWebCamNotAvaliable = "Выбранная веб камера не доступна";
			CantOpenGpsOnPort = "Не удается открыть GPS приемник на порту '{0}'. GPS не активен. Описание: {1}";
		}

		static private void InitEnglishResources()
		{
			Speed = "Speed:";
			Kmh = "km/h";
			Satellites = "Satellites:";
			NoGpsSignal = "No GPS signal";
			GpsNotConnected = "GPS not connected";
			Stop = "Stop";
			Start = "Start";
			Maximize = "Maximize";
			Normalize = "Normalize";
			OnStartDelay = "On start delay";
			CurrentWebCamNotAvaliable = "Current webcam is not avaliable";
			CantOpenGpsOnPort = "Can't open GPS receiver on port '{0}'. GPS not active. Description: {1}";
		}			 
			
		static public void InitDynamicResources(string language)
		{
			if (language == "English")
				InitEnglishResources();
			else if (language == "Russian")
				InitRussianResources();
		}
	}
}
