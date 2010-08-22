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
        public void AddToCurrent(ref Bitmap frame)
		{
			lock (aviWatchDog)
			{
				currentAvi.AddFrame(frame);
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
    }

	class VideoSplitter
	{
		private System.Timers.Timer timerSplit;
		private int secondsElapsed = 0;
		private bool nextAviPrepared = false;

		private object secondsWatchDog = new object();
		private object aviWatchDog = new object();

		private AVIWritersPair avipair = new AVIWritersPair();

		#region Path
		private string path = string.Empty;
		public string Path
		{
			get { return path; }
			set { path = value; }
		}
		#endregion

		#region FileDuration
		private int fileDuration = 10*60;
		public int FileDuration
		{
			get { return fileDuration / 60; }
			set { fileDuration = value * 60; }
		}
		#endregion

		#region Number of cycled files
		private int numberOfFiles = 10;
		public int NumberOfFiles
		{
			get { return numberOfFiles; }
			set { numberOfFiles = value; }
		}
		#endregion

		#region Codec
		private string codec = string.Empty;
		public string Codec
		{
			get { return codec; }
			set { codec = value; }
		}
		#endregion

		#region Frapes Per Second
		private int fps = 25;
		public int FPS
		{
			get { return fps; }
			set { fps = value; }
		}
		#endregion

		#region Video Size
		private Size videoSize = Size.Empty;
		public Size VideoSize
		{
			get { return videoSize; }
			set { videoSize = value; }
		}
		#endregion

		public VideoSplitter()
		{
			timerSplit = new System.Timers.Timer();
			timerSplit.Interval = 1000;
			timerSplit.Elapsed += new ElapsedEventHandler(timerSplit_Elapsed);
		}

		void timerSplit_Elapsed(object sender, ElapsedEventArgs e)
		{
			++secondsElapsed;
		}
		
		public void AddFrame(ref Bitmap frame)
		{
			lock (secondsWatchDog)
			{
				// before 10 seconds, open new avi
				if (!nextAviPrepared && (secondsElapsed % fileDuration) == (fileDuration - 9))
				{
					nextAviPrepared = true;
					new Thread(PrepareNewMovie).Start();
				}

				if (nextAviPrepared && (secondsElapsed % fileDuration) == 0)
				{
					nextAviPrepared = false;
					avipair.Replace();
					new Thread(ClosePreparedAvi).Start();
				}
			}

			avipair.AddToCurrent(ref frame);
		}

		public void Start()
		{
			avipair.Codec = codec;
			avipair.FrameRate = fps;

			nextAviPrepared = false;
			StartNewMovie(0);
			timerSplit.Start();
		}

		public void Stop()
		{
			timerSplit.Stop();
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

		private void StartNewMovie(int oneOfAvi)
		{
			// if preparing, next avi will started after 10 seconds
			string filename = oneOfAvi == 1 ? DateTime.Now.AddSeconds(9).ToString() : DateTime.Now.ToString();

			filename = path + "\\CarDVR_" +
					   filename.Replace(':', '_').Replace(' ', '_').Replace('.', '_');

			try
			{
				AVIWriter avi = oneOfAvi == 0 ? avipair.GetCurrent() : avipair.GetPrepared();
				avi.Open(filename + ".avi", videoSize.Width, videoSize.Height);
			}
			catch (Exception)
			{
				Reporter.Error("Can't open source with resolution " + videoSize.Width.ToString() + "X" + videoSize.Height.ToString());
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

		#region Helpers
		private class FileInfoComparer : IComparer<FileInfo>
		{
			public int Compare(FileInfo x, FileInfo y)
			{
				if (x.CreationTime == y.CreationTime)
					return 0;

				if (x.CreationTime > y.CreationTime)
					return -1;

				if (x.CreationTime < y.CreationTime)
					return 1;

				return 0;
			}
		}
		#endregion
	}
}
