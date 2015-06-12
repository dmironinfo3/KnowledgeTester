using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using KT.DTOs;

namespace KT.EmailSender
{
	public class EmailSender : IEmailSender
	{
		public void Send(Email dto)
		{
			var mail = new MailMessage(dto.From, dto.To);

			var client = new SmtpClient
			{
				Port = dto.Port,
				DeliveryMethod = dto.DeliverMethod,
				UseDefaultCredentials = dto.UseDefaultCredentials,
				Host = dto.Host
			};

			mail.Subject = dto.Subject;
			mail.Body = dto.Body;
			client.Send(mail);
		}
	}
}
