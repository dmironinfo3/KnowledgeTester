using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using KT.DB.Helpers;
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

			SessionWrapper.UserIsAdmin = IsAdmin(pageModel.UserName, pageModel.Password);

			if (SessionWrapper.UserIsAdmin)
			{
				return RedirectToAction("Index", "AdminPanel");
			}

			var valid = ServicesFactory.GetService<IKtUsersService>().
			Authenticate(pageModel.UserName, pageModel.Password);

			if (valid)
			{
				SessionWrapper.Student = ServicesFactory.GetService<IKtUsersService>().GetByKey(pageModel.UserName);
				return RedirectToAction("Index", "StudentPanel");
			}

			return View("Index", pageModel);
		}

		private bool IsAdmin(string uname, string pas)
		{
			var configName = ConfigurationManager.AppSettings["adminUser"];
			var configPass = ConfigurationManager.AppSettings["adminPass"];

			return uname.Equals(configName) && pas.Equals(configPass);
		}

		[HttpPost]
		public ActionResult Delete(string id)
		{
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Get()
		{
			var sss = new List<LoginModel>()
				{
					new LoginModel(){PassHint = "p1", Password =  "p2",UserName = "u22"},
					new LoginModel(){PassHint = "p1", Password =  "p2",UserName = "u22"},
					new LoginModel(){PassHint = "p1", Password =  "p2",UserName = "u22"},
					new LoginModel(){PassHint = "p1", Password =  "p2",UserName = "u22"},
					new LoginModel(){PassHint = "p1", Password =  "p2",UserName = "u22"},
					new LoginModel() {PassHint = "p2", Password =  "p33",UserName = "u333"}
				};


			var result = new
			{
				total = (int)Math.Ceiling((double)sss.Count / 10),
				page = 1,
				records = sss.Count,
				rows = (from row in sss
						
						select new
						{
							row.PassHint,
							row.Password,
							row.UserName
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
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
