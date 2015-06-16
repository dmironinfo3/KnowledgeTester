using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using KT.Logger.ObserverPattern;

namespace KT.Logger
{
	/// <summary>
	/// Is handling the process that writes to the log file
	/// </summary>
	public static class LogHandler
	{
		private static bool _isActive;

		public static int QueueCapacity;

		public static void Init(string level, int queueCapacity)
		{
			_level = GetLevel(level);
			_logQueue = new Queue();
			_logQueue.Init();

			_isActive = true;

			QueueCapacity = queueCapacity;

			AttachObservers();
		}

		public static void Append(Entry entry)
		{
			if (_isActive && entry.Level >= _level)
			{
				_logQueue.AddEntry(entry);
			}
		}

		private static Level GetLevel(string lvl)
		{
			switch (lvl.ToUpper())
			{
				case "ERROR":
					return Level.Error;
				case "DEBUG":
					return Level.Debug;
				case "INFO":
					return Level.Info;
				default:
					return Level.Unknown;
			}
		}

		private static void AttachObservers()
		{
			var observer = LogObserver.GetAll();

			foreach (var logObserver in observer)
			{
				_logQueue.Attach(logObserver);
			}
		}

		private static Queue _logQueue;
		private static Level _level;
	}
}
