using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using KT.Services.Helpers;
using KT.Services.Ninject;

namespace KT.Services
{
	public class Global : HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			//Email Sender
			KtEmailSender.Init();

			//Binding kernel
			var kernel = (new Bindings()).LoadNinjectBindings();

			//Init service factory which will provide needed service on demand
			ServicesFactory.Init(kernel);
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}