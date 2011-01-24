using System;
using System.Collections.Generic;
using System.Text;
using AForge.Video.DirectShow;
using System.Drawing;
using AForge.Video;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace CarDVR
{
	public class VideoManager
	{
		private static readonly Font framefont = new Font("Arial", 8, FontStyle.Bold);
		private static readonly Point pointWhite = new Point(5, 5);
		private static readonly Point pointBlack = new Point(6, 6);

		// TODO: make stand alone class FramesCounter
		int lastFrames = 0, totalFrames = 0, lastFps = 0;
		object framesCountKeeper = new object();

		VideoCaptureDevice webcam = null;
		public Bitmap frame = null;
		public object frameKeeper = new object();

		VideoSplitter splitter = new VideoSplitter();

		System.Timers.Timer FpsDisplayer = new System.Timers.Timer();
		System.Threading.Thread writeThread = null;

		int writeDelay = 0;
		int writtenFrames = 0;

		GpsReceiver gps;

		public NewFrameEventHandler NewFrame;

		public VideoManager(GpsReceiver gpsRcvr)
		{
			gps = gpsRcvr;

			FpsDisplayer.Interval = 1000;
			FpsDisplayer.Elapsed += new System.Timers.ElapsedEventHandler(FpsDisplayer_Tick);
			FpsDisplayer.Enabled = false;
		}

		public void WriteThreadProc()
		{
			Stopwatch watch = new Stopwatch();

			long delay;
			long last = 0;

			while (true)
			{
				watch.Start();

				lock (frameKeeper)
				{
					if (frame == null)
						continue;

					if (Program.settings.EnableRotate)
					{
						switch (Program.settings.RotateAngle)
						{
							case 90:
								frame.RotateFlip(RotateFlipType.Rotate90FlipNone);
								break;
							case 180:
								frame.RotateFlip(RotateFlipType.Rotate180FlipNone);
								break;
							case 270:
								frame.RotateFlip(RotateFlipType.Rotate270FlipNone);
								break;
						}
					}

					using (Graphics graphics = Graphics.FromImage(frame))
					{
						string frameString = MakeFrameString();
						graphics.DrawString(frameString, framefont, Brushes.Black, pointBlack);
						graphics.DrawString(frameString, framefont, Brushes.White, pointWhite);
					}

					splitter.AddFrame(frame);
					++writtenFrames;
					++totalFrames;

					if (NewFrame != null)
						NewFrame(this, new NewFrameEventArgs(frame));

				}

				watch.Stop();

				delay = writeDelay + last - watch.ElapsedMilliseconds;

				if (delay > 0)
					System.Threading.Thread.Sleep((int)delay);

				last = watch.ElapsedMilliseconds;
			}
		}	 

		public void Close()
		{
			splitter.DisposeAll();
		}
		
		public bool IsRecording()
		{
			return webcam != null && webcam.IsRunning;
		}

		public bool SureThatWebcamExists(string device)
		{
			try
			{
				FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

				foreach (FilterInfo item in videoDevices)
				{
					if (item.MonikerString == device)
						return true;
				}
			}
			catch { }

			return false;
		}

		public void Initialize()
		{
			// locking frameKeeper to prevent using video source
			lock (frameKeeper)
			{
				// demension error protection in Writer timer
				frame = null;

				if (webcam != null)
					webcam.NewFrame -= videoSource_NewFrame;

				if (!SureThatWebcamExists(Program.settings.VideoSourceId))
					throw new NoWebcamException();

				int fps = Program.settings.OutputRateFps;

				if (fps > Program.settings.VideoFps)
					fps = Program.settings.VideoFps;

				if (fps == 0)
					fps = 25;
				
				webcam = new VideoCaptureDevice(Program.settings.VideoSourceId);
				webcam.NewFrame += videoSource_NewFrame;
				webcam.DesiredFrameRate = Program.settings.VideoFps > 0 ? Program.settings.VideoFps : fps;
				webcam.DesiredFrameSize = new Size(Program.settings.VideoWidth, Program.settings.VideoHeight);

				splitter.Codec = Program.settings.Codec;
				splitter.FPS = fps;
				splitter.VideoSize = Program.settings.GetVideoSize();
				splitter.FileDuration = Program.settings.AviDuration;
				splitter.NumberOfFiles = Program.settings.AmountOfFiles;
				splitter.Path = Program.settings.PathForVideo;

				writeDelay = 1000 / fps;
			}
		}

		void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			lock (frameKeeper)
			{
				if (frame != null)
					frame.Dispose();

				frame = (Bitmap)eventArgs.Frame.Clone();
			}
		}

		public void Start()
		{
			splitter.Start();
			webcam.Start();

			writeThread = new Thread(new ThreadStart(WriteThreadProc));
			writeThread.Start();
			
			FpsDisplayer.Enabled = true;
		}

		public void Stop()
		{
			FpsDisplayer.Enabled = false;

			writeThread.Abort();
			writeThread.Join();
			writeThread = null;

			webcam.SignalToStop();
			webcam.WaitForStop();
			splitter.Stop();			
		}

		private string MakeFrameString()
		{
			string result = DateTime.Now.ToString() + " ";

			if (Program.settings.GpsEnabled)
			{
				switch (gps.State)
				{
					case GpsState.Active:
						result += MainForm.resSpeed + " " + gps.Speed + " " +
									MainForm.resKmh + " " +
									MainForm.resSatellites + " " + gps.NumberOfSatellites.ToString() + "\n" + gps.Coordinates;
						break;
					case GpsState.NoSignal:
						result += MainForm.resNoGpsSignal;
						break;
					case GpsState.NotActive:
						result += MainForm.resGpsNotConnected;
						break;
				}
			}

			lock (framesCountKeeper)
			{
				result += "\n" + totalFrames.ToString() + " | " + lastFps.ToString() + " FPS";
			}

			return result;
		}

		private void FpsDisplayer_Tick(object sender, EventArgs e)
		{
			lock (framesCountKeeper)
			{
				lastFps = totalFrames - lastFrames;
				lastFrames = totalFrames;
			}
		}

		public void ShowPpropertiesDialog(string moniker, Form parent)
		{
			try
			{
				new VideoCaptureDevice(moniker).DisplayPropertyPage(parent.Handle);
			}
			catch
			{
				throw new WebcamPropertiesException();
			}
		}
	}
}
