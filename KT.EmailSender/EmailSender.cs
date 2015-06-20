using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Mail;

namespace KT.EmailSender
{
	public class EmailSender : IEmailSender
	{
		public void Send(Email dto)
		{
			var mail = new MailMessage(dto.From, dto.To)
			{
				Subject = dto.Subject,
				Body = dto.Body,
				IsBodyHtml = true,
			};

			var smtp = new SmtpClient
			{
				Host = dto.Host,
				Port = dto.Port,
				Credentials = new NetworkCredential(dto.From, dto.FromPassword),
				EnableSsl = true
			};

			smtp.Send(mail);
		}
	}
}
