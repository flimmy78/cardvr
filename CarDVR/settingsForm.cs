using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using Microsoft.Win32;
using System.Reflection;
using AForge.Video.DirectShow;
using System.IO;

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
        }

        public void ApplyFormToSettings()
        {
			Program.settings.GpsEnabled = enableGps.Checked;
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
            }
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
    }

	public class SettingsImpl
	{
		// Registry path to our settings
		private static readonly string REG_PATH = "Software\\CarDVR";
        private static readonly string DEFAULT_PATH = "C:\\";

		#region SerialPort
		private string _GpsSerialPort = string.Empty;
		public string GpsSerialPort
		{
			get { return _GpsSerialPort; }
			set { _GpsSerialPort = value; }
		}
		#endregion

		#region BaudRate
		private int _SerialPortBaudRate = 0;
		public int SerialPortBaudRate
		{
			get { return _SerialPortBaudRate; }
			set { _SerialPortBaudRate = value; }
		}
		#endregion

		#region Video Source Name
		private string _VideoSource = string.Empty;
		public string VideoSource
		{
			get { return _VideoSource; }
			set { _VideoSource = value; }
		}
		#endregion

		#region Video Source Id
		private string _VideoSourceId = string.Empty;
		public string VideoSourceId
		{
			get { return _VideoSourceId; }
			set { _VideoSourceId = value; }
		}
		#endregion

        #region Video Resolution
        private int _VideoWidth = 0;
        public int VideoWidth
        {
            get { return _VideoWidth; }
            set { _VideoWidth = value; }
        }
        private int _VideoHeight = 0;
        public int VideoHeight
        {
            get { return _VideoHeight; }
            set { _VideoHeight = value; }
        }
        private string _VideoResolutionString = string.Empty;
        public string VideoResolutionString
        {
            get { return _VideoResolutionString; }
            set { _VideoResolutionString = value; }
        }
        #endregion

		#region GpsEnabled
		private bool _GpsEnabled = false;
		public bool GpsEnabled
		{
			get { return _GpsEnabled; }
			set { _GpsEnabled = value; }
		}
		#endregion

		#region StartWithWindows
		private bool _StartWithWindows = false;
		public bool StartWithWindows
		{
			get { return _StartWithWindows; }
			set { _StartWithWindows = value; }
		}
		#endregion

		#region AutostartRecording
		private bool _AutostartRecording = false;
		public bool AutostartRecording
		{
			get { return _AutostartRecording; }
			set { _AutostartRecording = value; }
		}
		#endregion

		#region Start Minimized
		private bool _StartMinimized = false;
		public bool StartMinimized
		{
			get { return _StartMinimized; }
			set { _StartMinimized = value; }
		}
		#endregion

        #region AVI duration
        private int _AviDuration = 10;
        public int AviDuration
        {
            get { return _AviDuration; }
            set { _AviDuration = value; }
        }
        #endregion

        #region Amount of files
        private int _AmountOfFiles = 10;
        public int AmountOfFiles
        {
            get { return _AmountOfFiles; }
            set { _AmountOfFiles = value; }
        }
        #endregion

        #region Path to save
        private string _PathForVideo = DEFAULT_PATH;
        public string PathForVideo
        {
            get { return _PathForVideo; }
            set { _PathForVideo = value; }
        }
        #endregion

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
