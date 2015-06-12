using System;

namespace KT.Logger
{
	public interface ILogger
	{
		void Debug(string message, string username = "");

		void Info(string message, string username = "");

		void Error(string message, string username = "", Exception ex = null);
	}
}
