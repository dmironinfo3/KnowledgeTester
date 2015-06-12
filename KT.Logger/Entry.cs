using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KT.Logger
{
	public class Entry
	{
		public DateTime Time { get; set; }
		public Level Level { get; set; }
		public string Username { get; set; }
		public string Message { get; set; }
		public Exception Exception { get; set; }

		public string ToEntryString()
		{
			var sb = (new StringBuilder()).Append(Time.ToShortDateString())
			.Append("\t")
			.Append(Time.ToShortTimeString())
			.Append("\t")
			.Append(GetLevel())
			.Append("\t")
			.Append(Username)
			.Append("\t")
			.Append(Message);

			if (Exception != null)
			{
				sb = sb.Append("\t")
				  .Append(Exception.Message);

				if (Exception.InnerException != null)
				{
					sb = sb.Append("\t")
						.Append(Exception.InnerException);
				}

			}

			return sb.ToString();
		}

		public static Entry New(Level lvl, string message, string username="", Exception exception = null)
		{
			var ent = new Entry()
				{
					Time = DateTime.Now,
					Level = lvl,
					Username = username,
					Message = message,
					Exception = exception
				};
			return ent;
		}

		private string GetLevel()
		{
			switch ((int)Level)
			{
				case 0:
					return "DEBUG";
				case 1:
					return "INFO";
				case 2:
					return "ERROR";
				default:
					return "UNKNOWN";
			}
		}
	}
}
