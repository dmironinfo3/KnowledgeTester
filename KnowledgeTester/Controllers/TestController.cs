using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.ServiceInterfaces;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;

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
			var subcategories = ServicesFactory.GetService<IKtSubcategoriesService>().GetAll().
				Select(a => new SubcategoryModel(a, string.Empty)).ToList();

			if (id.Value.Equals(Guid.Empty))
			{
				var m = new TestModel { Subcategories = subcategories };
				return View(m);
			}

			var test = ServicesFactory.GetService<IKtTestService>().GetById(id.Value);

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
				model.Subcategories = ServicesFactory.GetService<IKtSubcategoriesService>().GetAll().
					Select(a => new SubcategoryModel(a, string.Empty)).ToList();

				ViewBag.Message = string.Empty;

				foreach (var val in ModelState.Values)
				{
					if (val.Errors.Count > 0)
					{
						foreach (var error in val.Errors)
						{
							ViewBag.Message += error.ErrorMessage + "\n\n";
						}
					}
				}

				ViewBag.Message += "Please review and try again!";
				return View("Index", model);
			}

			var id = ServicesFactory.GetService<IKtTestService>().Save(model.Name, model.StartDate, model.EndDate, model.Duration, model.SubcategoryId, model.Questions, model.Id);

			return RedirectToAction("Index", "Test", new { id });
		}
	}
}
