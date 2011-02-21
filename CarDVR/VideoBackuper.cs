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

	class VideoBackuper
	{
		int copied_ = 0;
		EventHandler callback_ = null;
		FileInfo[] files_ = null;

		public int GetFilesCopied()
		{
			return copied_;
		}

		public VideoBackuper(FileInfo[] files, EventHandler callback)
		{
			files_ = files;
			callback_ = callback;
		}

		private void CopyThread()
		{
			copied_ = 0;
			string destination = Program.settings.BackupPath + "/";

			for (int index = 0; index < Program.settings.BackupFilesAmount && index < files_.Length; ++index)
			{
				try
				{
					File.Copy(files_[index].FullName, destination + files_[index].Name);
					++copied_;
				}
				catch { }
			}

			if (callback_ != null)
				callback_(this, EventArgs.Empty);
		}

		public void Do()
		{
			Thread thread = new Thread(new ThreadStart(CopyThread));
			thread.Priority = ThreadPriority.Lowest;
			thread.Start();
		}
	}
}
