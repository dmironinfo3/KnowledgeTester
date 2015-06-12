using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using KT.DTOs;

namespace KT.EmailSender
{
	public class EmailBuilder
	{
		public static Email Build(string from, string to, string subject, string body)
		{
			return new Email()
			{
				From = from,
				To = to,
				Body = body,
				DeliverMethod = DeliveryMethod,
				Host = Host,
				Port = Port,
				Subject = subject,
				UseDefaultCredentials = UseDefaultCredentials,
			};
		}

		public static bool UseDefaultCredentials
		{
			get
			{
				return true;//get from CONFIG
			}
		}

		public static int Port
		{
			get
			{
				return 25;//get from CONFIG

			}
		}
		
		public static string Host
		{
			get
			{
				return "smtp.google.com"; //get from CONFIG
			}
		}

		public static SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				var deliveryMethod = "";//get FROM CONFIG
				switch (deliveryMethod)
				{
					case "PickupDirectoryFromIis":
						return SmtpDeliveryMethod.PickupDirectoryFromIis;
					case "SpecifiedPickupDirectory":
						return SmtpDeliveryMethod.SpecifiedPickupDirectory;
					case "Network":
						return SmtpDeliveryMethod.Network;
				}

				return SmtpDeliveryMethod.Network;
			}
		}
	}
}
