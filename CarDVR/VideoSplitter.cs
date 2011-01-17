using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Timers;
using AForge.Video.VFW;

namespace CarDVR
{
	// Pair of two AVIWriters, opening in own threads
	class AVIWritersPair
	{
		private AVIWriter currentAvi = new AVIWriter();
		private AVIWriter preparedAvi = new AVIWriter();
		private object aviWatchDog = new object();

		#region Getters
		public AVIWriter GetCurrent()
		{
			return currentAvi;
		}

		public AVIWriter GetPrepared()
		{
			return preparedAvi;
		}
		#endregion

		#region Operations
		public void AddToCurrent(Bitmap frame)
		{
			lock (aviWatchDog)
			{
				try
				{
					currentAvi.AddFrame(frame);
				}
				catch (Exception e)
				{
					Reporter.NonSeriousError(e.Message);
					// frame was not added (may be shutdown)
				}
			}
		}

		public void Replace()
		{
			lock (aviWatchDog)
			{
				AVIWriter tmp = currentAvi;
				currentAvi = preparedAvi;
				preparedAvi = tmp;
			}
		}
		#endregion

		#region Codec
		public string Codec
		{
			set
			{
				currentAvi.Codec = value;
				preparedAvi.Codec = value;
			}
		}
		#endregion

		#region FrameRate
		public int FrameRate
		{
			set
			{
				currentAvi.FrameRate = value;
				preparedAvi.FrameRate = value;
			}
		}
		#endregion

		#region Closers
		public void CloseCurrent()
		{
			lock (aviWatchDog)
			{
				currentAvi.Close();
			}
		}

		public void ClosePrepared()
		{
			lock (aviWatchDog)
			{
				preparedAvi.Close();
			}
		}

		public void CloseAll()
		{
			lock (aviWatchDog)
			{
				currentAvi.Close();
				preparedAvi.Close();
			}
		}
		#endregion

		public void DisposeAll()
		{
			currentAvi.Dispose();
			preparedAvi.Dispose();
		}
	}

	class VideoSplitter
	{
		private System.Timers.Timer timerSplit;
		private int secondsElapsed = 0;
		private bool nextAviPrepared = false;
		private object secondsWatchDog = new object();
		private object aviWatchDog = new object();
		private AVIWritersPair avipair = new AVIWritersPair();

		public string Path { get; set; }
		public int FileDuration { get; set; }
		public int NumberOfFiles { get; set; }
		public string Codec { get; set; }
		public int FPS { get; set; }
		public Size VideoSize { get; set; }

		public VideoSplitter()
		{
			// default settings
			FileDuration = 10 * 60;
			NumberOfFiles = 10;

			timerSplit = new System.Timers.Timer();
			timerSplit.Interval = 1000;
			timerSplit.Elapsed += new ElapsedEventHandler(timerSplit_Elapsed);
		}

		void timerSplit_Elapsed(object sender, ElapsedEventArgs e)
		{
			++secondsElapsed;
		}

		public void AddFrame(Bitmap frame)
		{
			if (frame == null)
				return;

			lock (secondsWatchDog)
			{
				// before 10 seconds, open new avi
				if (!nextAviPrepared && (secondsElapsed % FileDuration) == (FileDuration - 9))
				{
					nextAviPrepared = true;
					new Thread(PrepareNewMovie).Start();
				}

				if (nextAviPrepared && (secondsElapsed % FileDuration) == 0)
				{
					nextAviPrepared = false;
					avipair.Replace();
					new Thread(ClosePreparedAvi).Start();
				}
			}

			avipair.AddToCurrent(frame);
		}

		public void Start()
		{
			avipair.Codec = Codec;
			avipair.FrameRate = FPS;

			nextAviPrepared = false;
			StartNewMovie(0);
			timerSplit.Start();
		}

		public void Stop()
		{
			timerSplit.Enabled = false;
			CloseAll();
		}

		private void PrepareNewMovie()
		{
			StartNewMovie(1);
		}

		private void CloseCurrentAvi()
		{
			avipair.CloseCurrent();
		}

		private void ClosePreparedAvi()
		{
			avipair.ClosePrepared();
		}

		private void CloseAll()
		{
			avipair.CloseAll();
		}

		public void DisposeAll()
		{
			avipair.DisposeAll();
		}

		private void StartNewMovie(int oneOfAvi)
		{
			// if preparing, next avi will started after 10 seconds
			string filename = oneOfAvi == 1 ? DateTime.Now.AddSeconds(9).ToString() : DateTime.Now.ToString();

			filename = Path + "\\CarDVR_" +
				filename.Replace(':', '_').Replace(' ', '_').Replace('.', '_') + ".avi";

			try
			{
				AVIWriter avi = oneOfAvi == 0 ? avipair.GetCurrent() : avipair.GetPrepared();


				avi.Open(filename, VideoSize.Width, VideoSize.Height);
			}
			catch (Exception e)
			{
				Reporter.SeriousError
				(
					"Can't open output file " + filename + "\n" +
					e.Message
				);
			}

			DirectoryInfo dir = new DirectoryInfo(Program.settings.PathForVideo);
			FileInfo[] files = dir.GetFiles("*.avi");

			Array.Sort<FileInfo>(files, new FileInfoComparer());

			for (int index = Program.settings.AmountOfFiles; index < files.Length; ++index)
			{
				try
				{
					File.Delete(files[index].FullName);
				}
				catch (Exception) { }
			}
		}

		private class FileInfoComparer : IComparer<FileInfo>
		{
			public int Compare(FileInfo x, FileInfo y)
			{
				return -x.CreationTime.CompareTo(y.CreationTime);
			}
		}
	}
}
