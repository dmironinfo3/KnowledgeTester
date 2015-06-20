using System.Collections.Generic;

namespace KT.Logger.ObserverPattern
{
	internal abstract class Subject
	{
		public List<LogObserver> Observers = new List<LogObserver>();

		internal void Attach(LogObserver logObserver)
		{
			Observers.Add(logObserver);
		}

		internal void Detach(LogObserver logObserver)
		{
			Observers.Remove(logObserver);
		}

		protected void Notify(IEnumerable<Entry> entries)
		{
			foreach (var o in Observers)
			{
				o.Observe(entries);
			}
		}
	}
}
