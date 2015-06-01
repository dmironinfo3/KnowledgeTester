using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.DB;
using KT.DB.Objects;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;

namespace KnowledgeTester.Controllers
{
	public class StudentPanelController : Controller
	{
		//
		// GET: /StudentPanel/

		public ActionResult Index()
		{
			if (!SessionWrapper.UserIsLoggedIn)
			{
				return RedirectToAction("Index", "Home");
			}

			var student = KT.DB.Helpers.Students.GetWithTests(SessionWrapper.Student.Username);

			var model = new StudentPanelModel(student);

			return View(model);
		}

		public ActionResult GetMyTests()
		{
			var tests = KT.DB.Helpers.Tests.GetAllUpcoming(SessionWrapper.Student.Username);

			var result = new
			{
				total = (int)Math.Ceiling((double)tests.Count / 10),
				page = 1,
				records = tests.Count,
				rows = (from row in tests
						orderby row.StartDate descending
						select new
						{
							Id = row.Id,
							Name = row.Name.Substring(0, row.Name.Length < 25 ? row.Name.Length : 25) + (row.Name.Length < 25 ? string.Empty : "..."),
							StartDate = row.StartDate.ToString("dd.MM.yy HH:mm"),
							Subscriptions = row.Students.Count,
							Duration = row.MinutesDuration,
							Subcategory = row.Subcategory.Name,
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetFinishedTests()
		{
			var tests = KT.DB.Helpers.Tests.GetFinishedTests(SessionWrapper.Student.Username).ToList();

			var result = new
			{
				total = (int)Math.Ceiling((double)tests.Count / 10),
				page = 1,
				records = tests.Count,
				rows = (from row in tests
						orderby row.StartDate descending
						select new
						{
							Id = row.Id,
							Name = row.Name.Substring(0, row.Name.Length < 25 ? row.Name.Length : 25) + (row.Name.Length < 25 ? string.Empty : "..."),
							StartDate = row.StartDate.ToString("dd.MM.yy HH:mm"),
							Score = KT.DB.Helpers.StudentTests.GetScore(row.Id, SessionWrapper.Student.Username),
							Subcategory = row.Subcategory.Name,
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetTests()
		{
			var tests = KT.DB.Helpers.Tests.GetAll();

			var result = new
			{
				total = (int)Math.Ceiling((double)tests.Count / 10),
				page = 1,
				records = tests.Count,
				rows = (from row in tests.Where(a => !a.Students.Any(st => st.Username.Equals(SessionWrapper.Student.Username)))
						orderby row.StartDate descending
						select new
						{
							Id = row.Id,
							Name = row.Name.Substring(0, row.Name.Length < 25 ? row.Name.Length : 25) + (row.Name.Length < 25 ? string.Empty : "..."),
							StartDate = row.StartDate.ToString("dd.MM.yy HH:mm"),
							Subscriptions = row.Students.Count,
							Duration = row.MinutesDuration,
							Subcategory = row.Subcategory.Name,
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult Subscribe(Guid id)
		{
			KT.DB.Helpers.Students.Subscribe(SessionWrapper.Student.Username, id);
			return Json("Succesfully subscribed!", JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult Unsubscribe(Guid id)
		{
			KT.DB.Helpers.Students.Unsubscribe(SessionWrapper.Student.Username, id);
			return Json("Succesfully unsubscribed!", JsonRequestBehavior.AllowGet);
		}
	}
}
