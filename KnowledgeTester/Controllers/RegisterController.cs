using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;

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

			if (ServicesFactory.GetService<IKtUsersService>().IsStudentExistent(pagemodel.Username))
			{
				ViewBag.Message = "This username is already registered in our database!";
				return View("Index");
			}

			var st = ServicesFactory.GetService<IKtUsersService>().Insert(new UserDto
				{
					Username = pagemodel.Username,
					Email = pagemodel.Email,
					Password = pagemodel.Password,
					PasswordHint = pagemodel.PassHint,
					FirstName = pagemodel.Firstname,
					LastName = pagemodel.LastName,
					IsAdmin = false
				});

			SessionWrapper.User = st;
			return RedirectToAction("Index", "StudentPanel");
		}
	}
}
