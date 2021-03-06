﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using KT.Logger;
using KnowledgeTester.Code;
using KnowledgeTester.Ninject;
using KnowledgeTester.WCFServices;

namespace KnowledgeTester
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AuthConfig.RegisterAuth();

			//WCF service
			KtServices.Init();

			//Logger Section
			KtLogger.Init();

			//ExcelHelper Section
			KtExcelHandler.Init();

			//Binding kernel
			var kernel = (new Bindings()).LoadNinjectBindings();

			//Init service factory which will provide needed service on demand
			ServicesFactory.Init(kernel);
		}
	}
}