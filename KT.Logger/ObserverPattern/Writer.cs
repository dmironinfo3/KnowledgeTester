using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace KT.Logger.ObserverPattern
{
	public class Writer : LogObserver
	{
		public override void Observe(IEnumerable<Entry> entries)
		{
			var file = GetFile();

			if (!String.IsNullOrEmpty(file) && File.Exists(file))
			{
				File.AppendAllLines(file, entries.Select(a=>a.ToEntryString()));
			}
		}

		private static string GetFile()
		{
			var path = Path.Combine(Environment.CurrentDirectory, FileRelativePath);

			var files = Directory.GetFiles(path, "ktLog*.txt").Select(a => new FileInfo(a)).OrderByDescending(a => a.CreationTime);

			if (!files.Any() || files.First().Length / 1048576 >= FileSizeMb)
			{
				return CreateNewFile();
			}

			return Path.Combine(Environment.CurrentDirectory, files.First().FullName);
		}

		private static string CreateNewFile()
		{
			var filePath = Path.Combine(Environment.CurrentDirectory, NewFileName);
			File.Create(filePath).Dispose();
			return filePath;
		}

		private static double? _fileSizeMb;
		public static double FileSizeMb
		{
			get
			{
				if (_fileSizeMb == null)
				{
					_fileSizeMb = Convert.ToDouble(ConfigurationManager.AppSettings["_fileSizeMb"]);
				}
				return _fileSizeMb.Value;
			}
		}

		private static string _fileRelativePath;
		public static string FileRelativePath
		{
			get
			{
				if (String.IsNullOrEmpty(_fileRelativePath))
				{
					_fileRelativePath = ConfigurationManager.AppSettings["_fileRelativePath"];
				}
				return _fileRelativePath;
			}
		}

		private static string NewFileName
		{
			get { return "ktLog_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".txt"; }
		}
	}
}
