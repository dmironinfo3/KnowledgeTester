using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KT.Logger;
using KT.ServiceInterfaces;
using KnowledgeTester.WCFServices;

namespace KnowledgeTester.Code
{
	public class KtLogger
	{
		private static ILogger _logger;

		public static ILogger Log
		{
			get { return _logger; }
		}

		public static void Init()
		{
			var logLevel = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("_logLevel");
			var queueCapacity = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("_queueCapacity");
			LogHandler.Init(logLevel, Convert.ToInt32(queueCapacity));

			_logger = new Logger();
		}
	}
}