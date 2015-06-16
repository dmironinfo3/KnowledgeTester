using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using KT.Logger.ObserverPattern;

namespace KT.Logger
{
	internal class Queue : Subject
	{
		private static ConcurrentQueue<Entry> _queue;
		private static bool _queueIsWrited;
		internal void Init()
		{
			_queue = new ConcurrentQueue<Entry>();
		}

		internal void AddEntry(Entry ent)
		{
			_queue.Enqueue(ent);

			Notify();
		}

		private void Notify()
		{
			if (_queue.Count >= QueueCapacity && !_queueIsWrited)
			{
				(new Thread(FreeQueue)).Start();
			}
		}

		private void FreeQueue()
		{
			_queueIsWrited = true;
			var entries = new List<Entry>();
			for (int i = 0; i < QueueCapacity; i++)
			{
				Entry ent;
				if (_queue.TryDequeue(out ent))
				{
					entries.Add(ent);
				}
			}
			Notify(entries);
			_queueIsWrited = false;
		}

		private static int? _queueCapacity;
		public static int QueueCapacity
		{
			get
			{
				if (_queueCapacity == null)
				{
					_queueCapacity = Convert.ToInt32(ConfigurationManager.AppSettings["_queueCapacity"]);
				}
				return _queueCapacity.Value;
			}
		}
	}
}
