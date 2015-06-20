using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KT.EmailSender
{
	public interface IEmailSender
	{
		void Send(Email dto);
	}
}
