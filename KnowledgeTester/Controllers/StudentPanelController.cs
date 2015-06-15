using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.ServiceInterfaces;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;
using KT.DTOs.Objects;

namespace KnowledgeTester.Controllers
{
	public class StudentPanelController : Controller
	{
		public ActionResult Index()
		{
			if (!SessionWrapper.UserIsLoggedIn)
			{
				return RedirectToAction("Index", "Home");
			}

			var student = ServicesFactory.GetService<IKtUsersService>().GetWithTests(SessionWrapper.User.Username);

			var model = new StudentPanelModel(student);

			return View(model);
		}

		public ActionResult GetMyTests(string name)
		{
			var tests = ServicesFactory.GetService<IKtTestService>().GetAllUpcoming(SessionWrapper.User.Username).ToList();

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
							Subscriptions = ServicesFactory.GetService<IKtUsersService>().GetSubscriptionsFor(row.Id),
							Duration = row.Duration,
							Subcategory = ServicesFactory.GetService<IKtTestService>().GetSubcategoryName(row.Id),
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetFinishedTests(string name)
		{
			var tests = ServicesFactory.GetService<IKtTestService>().GetFinishedTests(SessionWrapper.User.Username).ToList();

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
							Score = ServicesFactory.GetService<IKtUserTestsService>().GetScore(row.Id, SessionWrapper.User.Username),
							Subcategory = ServicesFactory.GetService<IKtTestService>().GetSubcategoryName(row.Id),
							IsValidated = ServicesFactory.GetService<IKtUserTestsService>().IsValidated(row.Id, SessionWrapper.User.Username),
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetTests(string name)
		{
			var tests = ServicesFactory.GetService<IKtTestService>().GetAllOtherThan(SessionWrapper.User.Username).ToList();

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
							Subscriptions = ServicesFactory.GetService<IKtUsersService>().GetSubscriptionsFor(row.Id),
							Duration = row.Duration,
							Subcategory = ServicesFactory.GetService<IKtTestService>().GetSubcategoryName(row.Id),
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult Subscribe(Guid id)
		{
			ServicesFactory.GetService<IKtUsersService>().Subscribe(SessionWrapper.User.Username, id);
			return Json("Succesfully subscribed!", JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult Unsubscribe(Guid id)
		{
			ServicesFactory.GetService<IKtUsersService>().Unsubscribe(SessionWrapper.User.Username, id);
			return Json("Succesfully unsubscribed!", JsonRequestBehavior.AllowGet);
		}
	}
}
