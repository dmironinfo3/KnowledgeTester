using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.DB;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;

namespace KnowledgeTester.Controllers
{
	public class RegisterController : Controller
	{
		//
		// GET: /Register/

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Register(RegisterModel pagemodel)
		{
			if (!ModelState.IsValid)
				return View("Index");

			if (KT.DB.Helpers.Students.IsStudentExistent(pagemodel.Username))
			{
				ViewBag.Message = "This username is already registered in our database!";
				return View("Index");
			}

			var st = KT.DB.Helpers.Students.Insert(new Student
				{
					Username = pagemodel.Username,
					Email = pagemodel.Email,
					Password = pagemodel.Password,
					PasswordHint = pagemodel.PassHint
				});

			SessionWrapper.Student = st;
			return RedirectToAction("Index", "StudentPanel");
		}
	}
}
