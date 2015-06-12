using System.Net.Mail;

namespace KT.EmailSender
{
	public class Email
	{
		public string From { get; set; }
		public string To { get; set; }
		public string Body { get; set; }
		public string Subject { get; set; }
		public bool UseDefaultCredentials { get; set; }
		public string Host { get; set; }
		public SmtpDeliveryMethod DeliverMethod { get; set; }
		public int Port { get; set; }
	}
}
