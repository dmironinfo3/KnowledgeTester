using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;

namespace KnowledgeTester.Controllers
{
	public class TestResultsController : Controller
	{
		//
		// GET: /TestResults/

		public ActionResult Index(Guid? id)
		{
			// user rights
			if (!SessionWrapper.UserIsAdmin)
			{
				return RedirectToAction("Index", "Home");
			}

			if (id == null || id == Guid.Empty)
			{
				ViewBag.Message = "Please select a valid test!";
				return RedirectToAction("Index", "AdminPanel");
			}

			var m = ServicesFactory.GetService<IKtUserTestsService>().GetTestResults(id.Value);

			var model = new TestResultsModel
			{
				Subscriptions = m.Subscriptions,
				AverageScore = m.AverageScore,
				Name = m.Name
			};

			SessionWrapper.CurrentTestResultId = id.Value;

			return View(model);
		}

		public ActionResult GetUsers(string name)
		{
			var s = ServicesFactory.GetService<IKtUserTestsService>()
				.GetTestResultRows(SessionWrapper.CurrentTestResultId).ToList();

			if (!String.IsNullOrEmpty(name))
			{
				s = s.Where(a => a.Username.Contains(name)).ToList();
			}
			var result = new
			{
				total = (int)Math.Ceiling((double)s.Count / 10),
				page = 1,
				records = s.Count,
				rows = (from row in s
						orderby row.Validated descending
						select new
						{
							Id = row.Username,
							FullName = row.FullName,
							Score = row.Score,
							Validated = row.Validated
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
