using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.DB;
using KT.DB.Objects;
using KT.ServiceInterfaces;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;

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

			var student = ServicesFactory.GetService<IKtUsersService>().GetWithTests(SessionWrapper.Student.Username);

			var model = new StudentPanelModel(student);

			return View(model);
		}

		public ActionResult GetMyTests()
		{
			var tests = ServicesFactory.GetService<IKtTestService>().GetAllUpcoming(SessionWrapper.Student.Username).ToList();

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

		public ActionResult GetFinishedTests()
		{
			var tests = ServicesFactory.GetService<IKtTestService>().GetFinishedTests(SessionWrapper.Student.Username).ToList();

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
							Score = ServicesFactory.GetService<IKtUserTestsService>().GetScore(row.Id, SessionWrapper.Student.Username),
							Subcategory = row.Subcategory.Name,
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetTests()
		{
			var tests = ServicesFactory.GetService<IKtTestService>().GetAll().ToList();

			var result = new
			{
				total = (int)Math.Ceiling((double)tests.Count / 10),
				page = 1,
				records = tests.Count,
				rows = (from row in tests.Where(a => !a.SubscribedUsers.Any(st => st.Username.Equals(SessionWrapper.Student.Username)))
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
		public ActionResult Subscribe(Guid id)
		{
			ServicesFactory.GetService<IKtUsersService>().Subscribe(SessionWrapper.Student.Username, id);
			return Json("Succesfully subscribed!", JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult Unsubscribe(Guid id)
		{
			ServicesFactory.GetService<IKtUsersService>().Unsubscribe(SessionWrapper.Student.Username, id);
			return Json("Succesfully unsubscribed!", JsonRequestBehavior.AllowGet);
		}
	}
}
