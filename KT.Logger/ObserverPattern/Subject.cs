using System.Collections.Generic;

namespace KT.Logger.ObserverPattern
{
	internal abstract class Subject
	{
		private List<LogObserver> _observers = new List<LogObserver>();

		internal void Attach(LogObserver logObserver)
		{
			_observers.Add(logObserver);
		}

		internal void Detach(LogObserver logObserver)
		{
			_observers.Remove(logObserver);
		}

		protected void Notify(IEnumerable<Entry> entries)
		{
			foreach (var o in _observers)
			{
				o.Observe(entries);
			}
		}
	}
}
