using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Timers;
using AForge.Video.VFW;

namespace CarDVR
{
	// Pair of two AVIWriters, working in own threads
	class AVIWritersPair
	{
		private AVIWriter currentAvi = new AVIWriter();
		private AVIWriter preparedAvi = new AVIWriter();
		private object aviWatchDog = new object();
		
		public string FileName { get; set; }
		public string PreparedFileName { get; set; }
		
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
		public void AddFrame(Bitmap frame)
		{
			lock (aviWatchDog)
			{
				try
				{
					if (frame == null)
						currentAvi.AddEmptyFrame();
					else
						currentAvi.AddFrame(frame);
				}
				catch (Exception e)
				{
					Reporter.UnseriousError(e.Message);
				}
			}
		}

		public void MakePreparedWriterActive()
		{
			lock (aviWatchDog)
			{
				AVIWriter tmp = currentAvi;
				currentAvi = preparedAvi;
				preparedAvi = tmp;
				FileName = PreparedFileName;
			}
		}

		private bool IsDonePrepare()
		{
			return FileName == PreparedFileName;
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
				try
				{
					if (IsDonePrepare())
						return;

					if (File.Exists(PreparedFileName))
						File.Delete(PreparedFileName);
				}
				catch { }
			}
		}

		public void CloseAll()
		{
			CloseCurrent();
			ClosePrepared();
		}
		#endregion

		#region CheckFileFlushedBeforeClose
		System.Timers.Timer disposeTimer = new System.Timers.Timer();
		bool everythingIsGood = false;

		public void DisposeAll()
		{
			CloseAll();

			currentAvi.Dispose();
			preparedAvi.Dispose();

			everythingIsGood = false;
			disposeTimer.Interval = 1000;
			disposeTimer.Elapsed += new ElapsedEventHandler(disposeTimer_Elapsed);
			disposeTimer.Enabled = true;

			while (true)
			{
				lock (this)
				{
					if (everythingIsGood)
						break;
				}

				Thread.Sleep(100);
			}

			disposeTimer.Enabled = false;
		}

		void disposeTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			lock (this)
			{
				if (everythingIsGood)
					return;

 				FileInfo info = new FileInfo(FileName);
				try
				{
					FileStream fs = info.OpenWrite();
					fs.Close();
					everythingIsGood = true;
				}
				catch { }
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
		private const int PREPARE_BEFORE = 10;
		private enum VideoType { Current, Prepared }

		public string Path { get; set; }
		public int FileDuration { get; set; }
		public int NumberOfFiles { get; set; }
		public string Codec { get; set; }
		public int FPS { get; set; }
		public Size VideoSize { get; set; }
		
		public VideoSplitter()
		{
			FileDuration = 10 * 60;
			NumberOfFiles = 10;

			timerSplit = new System.Timers.Timer();
			timerSplit.Interval = 1000;
			timerSplit.Elapsed += new ElapsedEventHandler(timerSplit_Elapsed);
		}

		void timerSplit_Elapsed(object sender, ElapsedEventArgs e)
		{
			lock (secondsWatchDog)
			{
				++secondsElapsed;

				int secondsOfCurrentFile = secondsElapsed % FileDuration;

				if (nextAviPrepared && secondsOfCurrentFile == 0)
				{
					nextAviPrepared = false;
					avipair.MakePreparedWriterActive();
					new Thread(ClosePreparedAvi).Start();
				} 
				else if (!nextAviPrepared && secondsOfCurrentFile == (FileDuration - PREPARE_BEFORE))
				{
					nextAviPrepared = true;
					new Thread(PrepareNewMovie).Start();
				}
			}
		}

		public void AddFrame(Bitmap frame)
		{
			avipair.AddFrame(frame);
		}

		public void Start()
		{
			avipair.Codec = Codec;
			avipair.FrameRate = FPS;

			nextAviPrepared = false;
			secondsElapsed = 0;

			OpenVideo(VideoType.Current);
			timerSplit.Start();
		}

		public void Stop()
		{
			timerSplit.Enabled = false;
			CloseAll();
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

		private void PrepareNewMovie()
		{
			OpenVideo(VideoType.Prepared);
		}

		private void OpenVideo(VideoType kind)
		{
			string filename = MakeAviFileName(kind);
			try
			{
				AVIWriter avi;

				switch (kind)
				{
					case VideoType.Current:
					{
						avi = avipair.GetCurrent();
						avipair.FileName = filename;
						break;
					}
					case VideoType.Prepared:
					{
						avi = avipair.GetPrepared();
						avipair.PreparedFileName = filename;
						break;
					}
					default:
						throw new VideoStartException();
				}					

				avi.Open(filename, VideoSize.Width, VideoSize.Height);
			}
			catch (Exception e)
			{
				Reporter.SeriousError
				(
					string.Format
					(
						Resources.CantOpenOutputFile,
						filename,
						e.Message
					)
				);
			}

			DeleteOldFiles();
		}

		private string MakeAviFileName(VideoType type)
		{
			string timeString = type == VideoType.Current ? DateTime.Now.ToString() : DateTime.Now.AddSeconds(PREPARE_BEFORE).ToString();
			return Path + "\\CarDVR_" + timeString.Replace(':', '_').Replace(' ', '_').Replace('.', '_') + ".avi";
		}

		private void DeleteOldFiles()
		{
			FileInfo[] files = FileInfoSorter.Get(Program.settings.PathForVideo);

			for (int index = Program.settings.AmountOfFiles + 1; index < files.Length; ++index)
			{
				try
				{
					File.Delete(files[index].FullName);
				}
				catch { }
			}
		}
	}
}
