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
using Newtonsoft.Json;

namespace KnowledgeTester.Controllers
{
	public class HomeController : Controller
	{

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

				if (SessionWrapper.UserIsAdmin)
				{
					return RedirectToAction("Index", "AdminPanel");
				}
				return RedirectToAction("Index", "StudentPanel");
			}
			return View("Index", pageModel);
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
			var value = ServicesFactory.GetService<IKtUsersService>().GetStudentHint(userName);

			return Json(value, JsonRequestBehavior.AllowGet);
		}
	}
}
