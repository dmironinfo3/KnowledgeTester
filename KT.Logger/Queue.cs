using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using KT.Logger.ObserverPattern;

namespace KT.Logger
{
	internal class Queue : Subject
	{
		private static ConcurrentQueue<Entry> _queue;

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
			var entries = new List<Entry>();

			if (_queue.Count >= QueueCapacity)
			{
				for (int i = 0; i < QueueCapacity; i++)
				{
					Entry ent;
					if (_queue.TryDequeue(out ent))
					{
						entries.Add(ent);
					}
				}
				Notify(entries);
			}
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
