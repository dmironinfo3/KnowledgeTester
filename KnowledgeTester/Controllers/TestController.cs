using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;

namespace KnowledgeTester.Controllers
{
	public class TestController : Controller
	{
		//
		// GET: /Test/

		public ActionResult Index(Guid? id)
		{
			// user rights
			if (!SessionWrapper.UserIsAdmin)
			{
				return RedirectToAction("Index", "Home");
			}

			if (id == null)
			{
				ViewBag.Message = "Please select a valid test!";
				return RedirectToAction("Index", "AdminPanel");
			}
			var subcategories = KT.DB.Helpers.Subcategories.GetAll().Select(a => new SubcategoryModel(a, string.Empty)).ToList();
			if (id.Value.Equals(Guid.Empty))
			{
				var m = new TestModel { Subcategories = subcategories };
				return View(m);
			}

			var test = KT.DB.Helpers.Tests.GetById(id.Value);

			if (test != null)
			{
				var m = new TestModel(test) { Subcategories = subcategories };
				return View(m);
			}

			ViewBag.Message = "Test does not exists!";

			return View();
		}

		[HttpPost]
		public ActionResult Save(TestModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("Index");
			}
			
			var id = KT.DB.Helpers.Tests.Save(model.Name, model.StartDate, model.EndDate, model.Duration, model.SubcategoryId, model.Id);

			return RedirectToAction("Index", "Test", new {id });
		}
	}
}
