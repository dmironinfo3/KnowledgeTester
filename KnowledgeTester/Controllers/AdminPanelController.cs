using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.ServiceInterfaces;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;

namespace KnowledgeTester.Controllers
{
	public class AdminPanelController : Controller
	{
		//
		// GET: /AdminPanel/

		public ActionResult Index()
		{
			var m = new AdminPanelModel();
			return View(m);
		}


		public ActionResult SaveNewCategory(AdminPanelModel newcategoryname)
		{
			return View("Index");
		}

		public ActionResult SaveNewSubcategory(AdminPanelModel newcategoryname)
		{
			return View("Index");
		}

		public ActionResult GetCategories()
		{
			var categories = ServicesFactory.GetService<IKtCategoriesService>().GetAll().ToList();

			var result = new
			{
				total = (int)Math.Ceiling((double)categories.Count / 10),
				page = 1,
				records = categories.Count,
				rows = (from row in categories
						orderby row.Name ascending
						select new
						{
							row.Id,
							row.Name,
							Subcategories = ServicesFactory.GetService<IKtSubcategoriesService>().GetCountByCategory(row.Id)
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult DeleteCategory(Guid id)
		{
			ServicesFactory.GetService<IKtCategoriesService>().Delete(id);

			return Json("Category is deleted!", JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetTests()
		{
			var tests = ServicesFactory.GetService<IKtTestService>().GetAll().ToList();

			var result = new
			{
				total = (int)Math.Ceiling((double)tests.Count / 10),
				page = 1,
				records = tests.Count,
				rows = (from row in tests
						orderby row.StartTime descending
						select new
						{
							Id = row.Id,
							Name = row.Name.Substring(0, row.Name.Length < 25 ? row.Name.Length : 25) + (row.Name.Length < 25 ? string.Empty : "..."),
							StartDate = row.StartTime.ToString("dd.MM.yy HH:mm"),
							Subscriptions = row.SubscribedUsers.Count(),
							Duration = row.Duration,
							Subcategory = row.Subcategory.Name,
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		public ActionResult DeleteTest(Guid id)
		{
			ServicesFactory.GetService<IKtTestService>().Delete(id);

			return Json("Test is deleted!", JsonRequestBehavior.AllowGet);
		}
	}
}
