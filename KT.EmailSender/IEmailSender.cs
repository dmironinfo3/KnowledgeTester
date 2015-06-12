using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KT.DTOs;

namespace KT.EmailSender
{
	public interface IEmailSender
	{
		void Send(Email dto);
	}
}
