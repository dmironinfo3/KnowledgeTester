using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace KT.EmailSender
{
	public class EmailBuilder
	{
		private static readonly string TemplateFileLocation =
			ConfigurationManager.AppSettings.Get("email_templateFileName");

		private static readonly string ConfigFrom =
		ConfigurationManager.AppSettings.Get("email_from");

		public static Email Build(string to, string subject, EmailTemplateInfo templateInfo)
		{
			return new Email()
			{
				From = ConfigFrom,
				To = to,
				Body = GetBody(templateInfo),
				DeliverMethod = DeliveryMethod,
				Host = Host,
				Port = Port,
				Subject = subject,
				FromPassword = FromPassword,
			};
		}

		public static IEmailSender GetSender()
		{
			return new EmailSender();
		}

		private static string GetBody(EmailTemplateInfo templateInfo)
		{
			var text = File.ReadAllText(TemplateFileLocation);

			text = text.Replace("#[FirstName]", templateInfo.FirstName);
			text = text.Replace("#[LastName]", templateInfo.LastName);
			text = text.Replace("#[TestName]", templateInfo.TestName);
			text = text.Replace("#[ProfFirstName]", templateInfo.ProfFirstName);
			text = text.Replace("#[ProfLastName]", templateInfo.ProfLastName);
			text = text.Replace("#[Score]", templateInfo.Score.ToString());
			text = text.Replace("#[MaxScore]", templateInfo.MaxScore.ToString());

			return text;
		}

		public static string FromPassword
		{
			get
			{
				return ConfigurationManager.AppSettings.Get("email_fromPassword");//get from CONFIG
			}
		}

		public static int Port
		{
			get
			{
				return Convert.ToInt32(ConfigurationManager.AppSettings.Get("email_port"));//get from CONFIG

			}
		}
		
		public static string Host
		{
			get
			{
				return ConfigurationManager.AppSettings.Get("email_host"); //get from CONFIG
			}
		}

		public static SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				var deliveryMethod = ConfigurationManager.AppSettings.Get("email_deliveryMethod");//get FROM CONFIG
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
