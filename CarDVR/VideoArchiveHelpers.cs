using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CarDVR
{
	class FileInfoComparer : IComparer<FileInfo>
	{
		public int Compare(FileInfo x, FileInfo y)
		{
			return -x.CreationTime.CompareTo(y.CreationTime);
		}
	}

	class FileInfoSorter
	{
		public static FileInfo[] Get(string path)
		{
			DirectoryInfo dir = new DirectoryInfo(path);
			FileInfo[] files = dir.GetFiles("*.avi");

			Array.Sort<FileInfo>(files, new FileInfoComparer());

			return files;
		}
	}
}
