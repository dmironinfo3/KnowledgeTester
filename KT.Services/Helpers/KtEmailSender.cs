using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KT.EmailSender;

namespace KT.Services.Helpers
{
	public class KtEmailSender
	{
		private static IEmailSender _sender;

		public static IEmailSender Sender
		{
			get { return _sender; }
		}

		public static void Init()
		{
			_sender = new EmailSender.EmailSender();
		}
	}
}