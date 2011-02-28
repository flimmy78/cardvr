using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace CarDVR
{
	enum BackupType
	{
		BackupAllFiles,
		BackupAllButCurrent
	};

	class ProgressEventArgs : EventArgs
	{
		public ProgressEventArgs(int progress, string text)
		{
			Progress = progress;
			Text = text;
		}

		public int Progress;
		public string Text;
	}

	delegate void ProgressEventHandler(object sender, ProgressEventArgs args);

	class VideoBackuper
	{
		int copied_ = 0;
		ProgressEventHandler progressCallback_ = null;
		EventHandler finishCallback_ = null;
		FileInfo[] files_ = null;

		public int GetFilesCopied()
		{
			return copied_;
		}

		public VideoBackuper(FileInfo[] files, ProgressEventHandler progressCallback, EventHandler finishCallback)
		{
			files_ = files;
			progressCallback_ = progressCallback;
			finishCallback_ = finishCallback;
		}

		private void CopyThread()
		{
			copied_ = 0;
			string destination = Program.settings.BackupPath + "/";

			try
			{
				Directory.CreateDirectory(destination);
			}
			catch (Exception e)
			{
				Reporter.SeriousError
				(
					string.Format
					(
						Resources.CantCreateDirectory,
						destination,
						e.Message
					)
				);
				DoFinish();
				return;
			}

			for (int index = 0; index < Program.settings.BackupFilesAmount && index < files_.Length; ++index)
			{
				try
				{
					if (progressCallback_ != null && Program.settings.BackupFilesAmount != 0)
					{
						progressCallback_
						(
							this,
							new ProgressEventArgs(index * 100 / Program.settings.BackupFilesAmount,
							string.Format(Resources.CopyingFile, files_[index].Name))
						);
					}

					File.Copy(files_[index].FullName, destination + files_[index].Name);
					++copied_;
				}
				catch { }
			}

			DoFinish();
		}

		private void DoFinish()
		{
			if (finishCallback_ != null)
				finishCallback_(this, EventArgs.Empty);
		}

		public void Do()
		{
			Thread thread = new Thread(new ThreadStart(CopyThread));
			thread.Priority = ThreadPriority.Lowest;
			thread.Start();
		}
	}
}
