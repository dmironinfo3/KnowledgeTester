using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KnowledgeTester.Code;
using KnowledgeTester.Helpers;
using KT.ServiceInterfaces;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;
using KT.DTOs.Objects;
using KT.Logger;
using Microsoft.Ajax.Utilities;

namespace KnowledgeTester.Controllers
{
	public class AdminPanelController : BaseController
	{
		//
		// GET: /AdminPanel/

		private static ILogger _log = ServicesFactory.GetService<ILogger>();

		public ActionResult Index()
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			var m = new AdminPanelModel();
			return View(m);
		}

		public ActionResult GetCategories(string catName)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			try
			{
				var categories = ServicesFactory.GetService<IKtCategoriesService>().GetAll().ToList();

				if (!String.IsNullOrEmpty(catName))
				{
					categories = categories.Where(a => a.Name.Contains(catName)).ToList();
				}

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
								Admin = row.CreatedBy.GetFullName(),
								Subcategories = ServicesFactory.GetService<IKtSubcategoriesService>().GetCountByCategory(row.Id)
							}).ToArray()
				};

				return Json(result, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, SessionWrapper.User.Username, ex);
				throw;
			}
		}

		[HttpPost]
		public ActionResult DeleteCategory(Guid id)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			try
			{
				ServicesFactory.GetService<IKtCategoriesService>().Delete(id);

				return Json("Category is deleted!", JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, SessionWrapper.User.Username, ex);
				throw;
			}
		}

		public ActionResult GetTests(string name)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			try
			{
				var tests = ServicesFactory.GetService<IKtTestService>().GetAll().ToList();

				if (!String.IsNullOrEmpty(name))
				{
					tests = tests.Where(a => a.Name.Contains(name)).ToList();
				}

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
								Subscriptions = ServicesFactory.GetService<IKtTestService>().GetSubscriptionsFor(row.Id),
								Duration = row.Duration,
								Subcategory = ServicesFactory.GetService<IKtTestService>().GetSubcategoryName(row.Id),
								Status = GetStatus(row)
							}).ToArray()
				};

				return Json(result, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, SessionWrapper.User.Username, ex);
				throw;
			}
		}

		private string GetStatus(TestDto row)
		{
			if (row.StartTime >= DateTime.Now)
			{
				return "Scheduled";
			}
			if (row.EndTime >= DateTime.Now)
			{
				return "In Progress";
			}
			if (row.EndTime < DateTime.Now)
			{
				return "Finished";
			}
			return "Unknown";
		}

		[HttpPost]
		public ActionResult DeleteTest(Guid id)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			try
			{
				ServicesFactory.GetService<IKtTestService>().Delete(id);

				return Json("Test is deleted!", JsonRequestBehavior.AllowGet);

			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, SessionWrapper.User.Username, ex);
				throw;
			}
		}
	}
}
