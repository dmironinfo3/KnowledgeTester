using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KT.Logger
{
	public class Logger : ILogger
	{
		public void Debug(string message, string username = "")
		{
			Create(Level.Error, message, username);
		}

		public void Info(string message, string username = "")
		{
			Create(Level.Error, message, username);
		}

		public void Error(string message, string username = "", Exception ex = null)
		{
			Create(Level.Error, message, username, ex);
		}

		private void Create(Level level, string message, string username = "", Exception ex = null)
		{
			var entry = Entry.New(level, message, username, ex);

			LogHandler.Append(entry);
		}
	}
}
