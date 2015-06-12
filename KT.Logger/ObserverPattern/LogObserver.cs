using System;
using System.Collections.Generic;
using System.Linq;

namespace KT.Logger.ObserverPattern
{
	public abstract class LogObserver
	{
		public abstract void Observe(IEnumerable<Entry> entries);

		internal static IEnumerable<LogObserver> GetAll()
		{
			IEnumerable<LogObserver> exporters = typeof(LogObserver)
				.Assembly.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(LogObserver)) && !t.IsAbstract)
				.Select(t => (LogObserver)Activator.CreateInstance(t));

			return exporters;
		}
	}
}
