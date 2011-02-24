using System;
using System.Collections.Generic;
using System.Text;
using AForge.Video.DirectShow;
using System.Drawing;
using AForge.Video;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

using Gps;

namespace CarDVR
{
	public class VideoManager
	{
		private static string FONT_NAME_ARIAL = "Arial";
		private static Font framefont = new Font(FONT_NAME_ARIAL, 8, FontStyle.Bold);
		private static readonly Point pointWhite = new Point(5, 5);
		private static readonly Point pointBlack = new Point(6, 6);

		private static readonly Point p1 = new Point(1, 1);
		private static readonly Point p2 = new Point(4, 1);
		private static readonly Point p3 = new Point(4, 4);
		private static readonly Point p4 = new Point(1, 4);

		private static readonly Point[] epicFailed = new Point[] { p1, p2, p3, p4};
		Pen epicPen = new Pen(Brushes.Red, 2);

		// TODO: make stand alone class FramesCounter
		int framesFromCamera = 0;
		int lastFramesFromCamera = 0;
		int fpsFromCamera = 0;

		int framesWritten = 0;
		int lastFramesWritter = 0;
		int fpsWritten = 0;

		int emptyFramesWritten = 0;
		int lastEmptyFramesWritten = 0;
		int fpsEmptyFrames = 0;

		object framesCountKeeper = new object();

		VideoCaptureDevice webcam = null;
		Bitmap frame = null;
		public object frameKeeper = new object();

		VideoSplitter splitter = new VideoSplitter();

		System.Timers.Timer FpsDisplayer = new System.Timers.Timer();
		System.Threading.Thread writeThread = null;

		int writeDelay = 0;
		bool frameWritten = false;
		int nullframecounter = 0;

		GpsReceiver gps;

		public NewFrameEventHandler NewFrame;

		Queue<Bitmap> writingQueue = new Queue<Bitmap>();

		object queueHolder = new object();
		Thread queueThread;
		AutoResetEvent writeEvent = new AutoResetEvent(false);

		public VideoManager(GpsReceiver gpsRcvr)
		{
			gps = gpsRcvr;
			FpsDisplayer.Elapsed += new System.Timers.ElapsedEventHandler(FpsDisplayer_Tick);
			FpsDisplayer.Interval = 1000;
		}

		// Thread that compressing video stream
		public void QueueProc()
		{
			while (true)
			{
				writeEvent.WaitOne();

				while (true)
				{
					Bitmap fr = null;
					int addEmptyFrames = 0;
					lock (queueHolder)
					{
						if (writingQueue.Count == 0)
							break;

						if (Program.settings.OutputRateFps > 2 && writingQueue.Count > Program.settings.OutputRateFps / 2)
						{
							addEmptyFrames = writingQueue.Count;
							writingQueue.Clear();														
						}
						else
						{
							fr = writingQueue.Dequeue();
						}
					}

					if (addEmptyFrames > 0)
					{
						while (addEmptyFrames > 0)
						{
							splitter.AddFrame(null);
							--addEmptyFrames;
							++emptyFramesWritten;
						}
						continue;
					}

					splitter.AddFrame(fr);

					if (fr != null)
					{
						++framesWritten;

						fr.Dispose();
						fr = null;
					}
					else
					{
						++emptyFramesWritten;
					}
				}
			}
		}

		public void WriteThreadProc()
		{
			Stopwatch watch = new Stopwatch();

			long delay = 0;
			long last = 0;

			while (true)
			{
				watch.Start();

				if (!EnqueuedEmptyFrameInsteadOfObsolete())
					PrepareFrameToEnqueue(delay);

				writeEvent.Set();
		 
				watch.Stop();

				delay = writeDelay + last - watch.ElapsedMilliseconds;

				if (delay > 0)
					System.Threading.Thread.Sleep((int)delay);

				last = watch.ElapsedMilliseconds;
			}
		}

		private bool EnqueuedEmptyFrameInsteadOfObsolete()
		{
			lock (queueHolder)
			{
				if (frameWritten)
				{
					++nullframecounter;
					writingQueue.Enqueue(null);
					return true;
				}
			}
			return false;
		}

		private void PrepareFrameToEnqueue(long oldDelay)
		{
			lock (frameKeeper)
			{
				if (frame == null)
					return;

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

					if (oldDelay < 0)
						graphics.DrawPolygon(epicPen, epicFailed);
				}

				if (NewFrame != null)
					NewFrame(this, new NewFrameEventArgs(frame));

				lock (queueHolder)
				{
					writingQueue.Enqueue((Bitmap)frame.Clone());
					frameWritten = true;
				}
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
				webcam.DesiredFrameRate = Program.settings.Cam1FrameRate > 0 ? Program.settings.Cam1FrameRate : fps;
				webcam.DesiredFrameSize = new Size(Program.settings.VideoWidth, Program.settings.VideoHeight);

				int writeFps = Program.settings.OutputRateFps != 0 ? Program.settings.OutputRateFps : fps;

				splitter.Codec = Program.settings.Codec;
				splitter.FPS = writeFps; // (Program.settings.Cam1FrameRate > 0 && Program.settings.Cam1FrameRate < fps ? Program.settings.Cam1FrameRate : fps);
				splitter.VideoSize = Program.settings.GetVideoSize();
				splitter.FileDuration = Program.settings.AviDuration;
				splitter.NumberOfFiles = Program.settings.AmountOfFiles;
				splitter.Path = Program.settings.PathForVideo;

				writeDelay = 1000 / (writeFps);

				int maxFrameSize = Program.settings.VideoWidth > Program.settings.VideoHeight 
									? Program.settings.VideoWidth : Program.settings.VideoHeight;
				int fontSize = maxFrameSize <= 800 ? 8 : maxFrameSize / 90;
				framefont = new Font(FONT_NAME_ARIAL, fontSize, FontStyle.Bold);
			}
		}

		void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			lock (frameKeeper)
			{
				if (frame != null)
					frame.Dispose();

				frame = (Bitmap)eventArgs.Frame.Clone();
				frameWritten = false;

				++framesFromCamera;
			}
		}

		public void Start()
		{
			splitter.Start();
			webcam.Start();

			queueThread = new Thread(new ThreadStart(QueueProc));
			queueThread.Start();

			writeThread = new Thread(new ThreadStart(WriteThreadProc));
			writeThread.Start();

			FpsDisplayer.Enabled = true;
		}

		public void Stop()
		{
			FpsDisplayer.Enabled = false;

			queueThread.Abort();
			queueThread.Join();
			queueThread = null;

			writeThread.Abort();
			writeThread.Join();
			writeThread = null;

			webcam.SignalToStop();
			webcam.WaitForStop();
			splitter.Stop();

			writingQueue.Clear();
		}

		private string MakeFrameString()
		{
			string result = DateTime.Now.ToString() + " ";

			if (Program.settings.GpsEnabled)
			{
				switch (gps.State)
				{
					case GpsState.Active:
						result += Resources.Speed + " " + gps.Speed + " " +
									Resources.Kmh + " " +
									Resources.Satellites + " " + gps.NumberOfSatellites.ToString() + "\n" + gps.Coordinates;
						break;
					case GpsState.NoSignal:
						result += Resources.NoGpsSignal;
						break;
					case GpsState.NotActive:
						result += Resources.GpsNotConnected;
						break;
				}
			}

			return result;
		}

		private void FpsDisplayer_Tick(object sender, EventArgs e)
		{
			lock (framesCountKeeper)
			{
				fpsFromCamera = framesFromCamera - lastFramesFromCamera;
				lastFramesFromCamera = framesFromCamera;

				fpsWritten = framesWritten - lastFramesWritter;
				lastFramesWritter = framesWritten;

				fpsEmptyFrames = emptyFramesWritten - lastEmptyFramesWritten;
				lastEmptyFramesWritten = emptyFramesWritten;
			}
		}

		public int FpsFromCamera()
		{
			int value = 0;

			lock (framesCountKeeper)
			{
				value = fpsFromCamera;
			}
			return value;
		}

		public int FpsWritten()
		{
			int value = 0;

			lock (framesCountKeeper)
			{
				value = fpsWritten;
			}
			return value;
		}

		public int FpsEmptyFrames()
		{
			int value = 0;

			lock (framesCountKeeper)
			{
				value = fpsEmptyFrames;
			}
			return value;
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
