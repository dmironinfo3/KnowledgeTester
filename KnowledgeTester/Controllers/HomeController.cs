using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using KT.ServiceInterfaces;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;
using KnowledgeTester.WCFServices;
using KT.Logger;
using Newtonsoft.Json;

namespace KnowledgeTester.Controllers
{
	public class HomeController : Controller
	{
		private static ILogger _log = ServicesFactory.GetService<ILogger>();

		public ActionResult Index()
		{
			if (SessionWrapper.UserIsAdmin)
			{
				return RedirectToAction("Index", "AdminPanel");
			}

			if (SessionWrapper.UserIsLoggedIn)
			{
				return RedirectToAction("Index", "StudentPanel");
			}

			return View(new LoginModel());
		}

		[HttpPost]
		public ActionResult Login(LoginModel pageModel)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View("Index", pageModel);
				}

				var valid = ServicesFactory.GetService<IKtUsersService>().
					Authenticate(pageModel.UserName, pageModel.Password);

				if (valid)
				{
					SessionWrapper.User = ServicesFactory.GetService<IKtUsersService>().GetByKey(pageModel.UserName);
					SessionWrapper.UserIsAdmin = SessionWrapper.User.IsAdmin;

					_log.Info("Logged in from Home Controller", SessionWrapper.User.Username);

					if (SessionWrapper.UserIsAdmin)
					{
						return RedirectToAction("Index", "AdminPanel");
					}
					return RedirectToAction("Index", "StudentPanel");
				}
				return View("Index", pageModel);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, SessionWrapper.User.Username, ex);
				throw;
			}

		}

		[HttpPost]
		public ActionResult Logout()
		{
			SessionWrapper.Logout();

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public ActionResult GetHint(string userName)
		{
			var value = ServicesFactory.GetService<IKtUsersService>().GetHint(userName);

			return Json(value, JsonRequestBehavior.AllowGet);
		}
	}
}
