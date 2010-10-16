﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Reflection;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using Microsoft.Win32;
using System.IO;
using System.Security.Permissions;
using System.Security;

namespace CarDVR
{
    public partial class settingsForm : Form
    {
        FilterInfoCollection videoDevices;

		public void ApplySettingsToForm()
		{
			List<string> portsList = new List<string>();

			#region CheckAvaliabilityOfSerialPort
			try
			{
				// Reading all avaliable serial ports
				portsList.AddRange( SerialPort.GetPortNames() );
			}
			catch (Win32Exception e)
			{
                Reporter.Error("Error occured during reading serial ports list.\nGPS won't be activated.\n Reason: " + e.Message);
				Program.settings.GpsEnabled = false;
			}

			if (portsList.Count == 0)
			{
				// Check any serial port avaliable
                Reporter.Error("There is no avaliable serial port in your system.\nGPS won't be activated.");
				Program.settings.GpsEnabled = false;
			}
			else if (!string.IsNullOrEmpty(Program.settings.GpsSerialPort) && !portsList.Contains(Program.settings.GpsSerialPort))
			{
				// Check saved serial port exists in system's serial ports list
                Reporter.Error("Serial port " + Program.settings.GpsSerialPort + " not found in system's serial ports list.\nGPS won't be activated.");
				Program.settings.GpsEnabled = false;
			}
			#endregion

			#region FillSerialPortList
			serialPortName.BeginUpdate();

			serialPortName.Items.Clear();
			serialPortName.Items.AddRange(portsList.ToArray());

			serialPortName.EndUpdate();
			#endregion

			#region Fill Rotate Angles
			comboRotateAngle.BeginUpdate();

			comboRotateAngle.Items.Clear();
			comboRotateAngle.Items.Add(90);
			comboRotateAngle.Items.Add(180);
			comboRotateAngle.Items.Add(270);

			comboRotateAngle.EndUpdate();
			#endregion

            #region Fill Baud rate
            serialPortBaudRate.BeginUpdate();
            serialPortBaudRate.Items.Clear();

            foreach (int baud in SerialPortBaudRates.values)
                serialPortBaudRate.Items.Add(baud);

            serialPortBaudRate.EndUpdate();
            #endregion

            #region Fill Video source
            videoSource.BeginUpdate();
            videoSource.Items.Clear();

            foreach (FilterInfo fi in videoDevices)
                videoSource.Items.Add(new VideoSourceInfo(fi.Name, fi.MonikerString));

            videoSource.EndUpdate();
            #endregion

            #region SetSerialPort
            foreach (object obj in serialPortName.Items)
            {
				if ((string)obj == Program.settings.GpsSerialPort)
                {
                    serialPortName.SelectedItem = obj;
                    break;
                }
            }
            #endregion

            #region SetBaudRate
            foreach (object obj in serialPortBaudRate.Items)
            {
				if ((int)obj == Program.settings.SerialPortBaudRate)
                {
                    serialPortBaudRate.SelectedItem = obj;
                    break;
                }
            }
            #endregion

            #region Set Video Source
            foreach (object obj in videoSource.Items)
            {
                if ((obj as VideoSourceInfo).Name == Program.settings.VideoSource)
                {
                    videoSource.SelectedItem = obj;
                    break;
                }
            }
            #endregion

			#region Set Rotate Angle
			foreach (object obj in comboRotateAngle.Items)
			{
				if ((int)obj == Program.settings.RotateAngle)
				{
					comboRotateAngle.SelectedItem = obj;
					break;
				}
			}
			#endregion

            // Fill resolution
            comboResolution_DropDown(this, EventArgs.Empty);

            #region Set Video Resolution
            foreach (object obj in comboResolution.Items)
            {
                if ((obj as CapInfo).Name == Program.settings.VideoResolutionString)
                {
                    comboResolution.SelectedItem = obj;
                    break;
                }
            }
            #endregion

			aviDuration.Value = Program.settings.AviDuration;
			amountOfFiles.Value = Program.settings.AmountOfFiles;

			enableGps.Checked = Program.settings.GpsEnabled;
			startWithWindows.Checked = Program.settings.StartWithWindows;
			autostartRecording.Checked = Program.settings.AutostartRecording;
			startMinimized.Checked = Program.settings.StartMinimized;
			textBoxPath.Text = Program.settings.PathForVideo;
			enableRotate.Checked = Program.settings.EnableRotate;
        }

        public void ApplyFormToSettings()
        {
			Program.settings.GpsEnabled = enableGps.Checked;
			Program.settings.EnableRotate = enableRotate.Checked;
			Program.settings.GpsSerialPort = serialPortName.Text;

            int baud;
            Int32.TryParse(serialPortBaudRate.Text, out baud);
			Program.settings.SerialPortBaudRate = baud;

			Program.settings.VideoSource = videoSource.Text;
			Program.settings.StartWithWindows = startWithWindows.Checked;
			Program.settings.AutostartRecording = autostartRecording.Checked;
			Program.settings.StartMinimized = startMinimized.Checked;

			Program.settings.VideoSourceId = string.Empty;
            foreach (FilterInfo fi in videoDevices)
				if (Program.settings.VideoSource == fi.Name)
                {
					Program.settings.VideoSourceId = fi.MonikerString;
                    break;
                }

			Program.settings.AmountOfFiles = (int)amountOfFiles.Value;
			Program.settings.AviDuration = (int)aviDuration.Value;
			Program.settings.PathForVideo = textBoxPath.Text;

            Program.settings.VideoResolutionString = comboResolution.Text;

            if (comboResolution.SelectedItem != null)
            {
                CapInfo capinfo = comboResolution.SelectedItem as CapInfo;
                Program.settings.VideoWidth = capinfo.Width;
                Program.settings.VideoHeight = capinfo.Height;
				Program.settings.VideoFps = capinfo.Frames;
            }

			if (comboRotateAngle.SelectedItem != null)
				Program.settings.RotateAngle = (int)comboRotateAngle.SelectedItem;
        }

        public settingsForm()
        {
			Program.settings = new SettingsImpl();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void LoadFromRegistry()
        {
			Program.settings.Read();
        }

        public void SaveToRegistry()
        {
			Program.settings.Save();
        }

        public SettingsImpl Settings
        {
            get 
            {
				return Program.settings;
            }
        }

        public bool CheckDirectoryForWrite(string dir)
        {
            if (!Directory.Exists(dir) || !DirectoryWriteChecker.Process(dir))
            {
                Reporter.Error("Can't write AVI file to directory " + dir);
                return false;
            }

            return true;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					textBoxPath.Text = dlg.SelectedPath;
            }
        }

        private void comboResolution_DropDown(object sender, EventArgs e)
        {
            comboResolution.BeginUpdate();
            comboResolution.Items.Clear();

            if (videoSource.SelectedItem != null)
            {
                VideoCaptureDevice vs = new VideoCaptureDevice((videoSource.SelectedItem as VideoSourceInfo).Moniker);

                foreach (VideoCapabilities cap in vs.VideoCapabilities)
                    comboResolution.Items.Add(new CapInfo(cap.FrameSize.Width, cap.FrameSize.Height, cap.MaxFrameRate));
            }
            comboResolution.EndUpdate();
        }

        private void settingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != System.Windows.Forms.DialogResult.OK)
                return;

            if (!CheckDirectoryForWrite(textBoxPath.Text))
                e.Cancel = true;
        }
    }

	public class SettingsImpl
	{
		// Registry path to our settings
		private static readonly string REG_PATH = @"Software\CarDVR";
        // Default path to store video files
        private static readonly string DEFAULT_PATH = @"C:\";

		public string GpsSerialPort	{ get; set;	}
		public int SerialPortBaudRate { get; set; }
		public string VideoSource { get; set; }
		public string VideoSourceId { get; set; }
        public int VideoWidth { get; set; }
        public int VideoHeight { get; set; }
		public int VideoFps { get; set; }
        public string VideoResolutionString { get; set; }
		public bool GpsEnabled { get; set; }
		public bool StartWithWindows { get; set; }
        public bool AutostartRecording { get; set; }
		public bool StartMinimized { get; set; }
        public int AviDuration { get; set; }
        public int AmountOfFiles { get; set; }
        public string PathForVideo { get; set; }
        public bool EnableRotate { get; set; }
		public int RotateAngle { get; set; }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        public SettingsImpl()
        {
            GpsSerialPort = string.Empty;
            SerialPortBaudRate = 0;
            VideoSource = string.Empty;
            VideoSourceId = string.Empty;
            VideoWidth = 0;
            VideoHeight = 0;
            VideoFps = 0;
            VideoResolutionString = string.Empty;
            GpsEnabled = false;
            StartWithWindows = false;
            AutostartRecording = false;
            StartMinimized = false;
            PathForVideo = DEFAULT_PATH;
            AmountOfFiles = 10;
            AviDuration = 10;
            EnableRotate = false;
            RotateAngle = 0;            
        }

		/// <summary>
		/// Read settings from the registry and applies to this class
		/// </summary>
		/// <returns>false if we have no rights to access registry</returns>
		public bool Read()
		{
			RegistryKey subkey;

			try
			{
				subkey = Registry.CurrentUser.OpenSubKey(REG_PATH);

				foreach (PropertyInfo pi in this.GetType().GetProperties())
				{
					try
					{
						pi.SetValue(this, Convert.ChangeType(subkey.GetValue(pi.Name), pi.PropertyType), null);
					}
					catch (Exception) { }
				}
				subkey.Close();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// This method will save all settings
		/// </summary>
		/// <returns>false if we have no rights to the registry</returns>
		public bool Save()
		{
			RegistryKey subkey;

			try
			{
				subkey = Registry.CurrentUser.OpenSubKey(REG_PATH, true);

				if (subkey == null)
					subkey = Registry.CurrentUser.CreateSubKey(REG_PATH);


				if (subkey == null)
					return false;

				foreach (PropertyInfo pi in this.GetType().GetProperties())
				{
					try
					{
						object o = pi.GetValue(this, null);

						subkey.SetValue(pi.Name, o);
					}
					catch (Exception e)
					{
						string s = e.Message;
					}
				}
				subkey.Close();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

        public Size GetVideoSize()
        {
			if (EnableRotate && (RotateAngle == 90 || RotateAngle == 270))
				return new Size(VideoHeight, VideoWidth);
			else
				return new Size(VideoWidth, VideoHeight);
        }
	}

    public class CapInfo
    {
        public string Name;
        public int Width;
        public int Height;
        public int Frames;

        public CapInfo(int w, int h, int f)
        {
            Width = w;
            Height = h;
            Frames = f;
            Name = w.ToString() + "X" + h.ToString() + "X" + f.ToString();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class VideoSourceInfo
    {
        public string Name;
        public string Moniker;

        public VideoSourceInfo(string n, string m)
        {
            Name = n;
            Moniker = m;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
