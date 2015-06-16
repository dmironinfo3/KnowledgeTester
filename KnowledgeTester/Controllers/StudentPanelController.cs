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
using KT.Logger;

namespace KnowledgeTester.Controllers
{
	public class StudentPanelController : BaseController
	{
		private static ILogger _log = ServicesFactory.GetService<ILogger>();

		public ActionResult Index()
		{
			try
			{
				if (!UserAllowed)
				{
					return RedirectToAction("Index", "Home");
				}

				var student = ServicesFactory.GetService<IKtUsersService>().GetWithTests(SessionWrapper.User.Username);

				var model = new StudentPanelModel(student);

				_log.Info("Login performed", student.Username);

				return View(model);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, SessionWrapper.User.Username, ex);
				throw;
			}
		}

		public ActionResult GetMyTests(string name)
		{
			if (!UserAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			try
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
								Subscriptions = ServicesFactory.GetService<IKtTestService>().GetSubscriptionsFor(row.Id),
								Duration = row.Duration,
								Subcategory = ServicesFactory.GetService<IKtTestService>().GetSubcategoryName(row.Id),
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

		public ActionResult GetFinishedTests(string name)
		{
			if (!UserAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			try
			{
				var tests = ServicesFactory.GetService<IKtTestService>()
					.GetFinishedTests(SessionWrapper.User.Username).ToList();

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
								Name =
									row.Name.Substring(0, row.Name.Length < 25 ? row.Name.Length : 25) +
									(row.Name.Length < 25 ? string.Empty : "..."),
								StartDate = row.StartTime.ToString("dd.MM.yy HH:mm"),

								Subcategory = ServicesFactory.GetService<IKtTestService>().GetSubcategoryName(row.Id),
								IsValidated = ServicesFactory.GetService<IKtUserTestsService>().IsValidated(row.Id, SessionWrapper.User.Username),
								Score = GetScore(row),
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

		private string GetScore(TestDto test)
		{
			var valid = ServicesFactory.GetService<IKtUserTestsService>().IsValidated(test.Id, 
				SessionWrapper.User.Username);

			if (!valid)
				return "- / " + test.Questions;

			var score = ServicesFactory.GetService<IKtUserTestsService>().GetScore(test.Id, SessionWrapper.User.Username);
			return score + " / " + test.Questions;
		}

		public ActionResult GetTests(string name)
		{
			if (!UserAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			try
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
								Subscriptions = ServicesFactory.GetService<IKtTestService>().GetSubscriptionsFor(row.Id),
								Duration = row.Duration,
								Subcategory = ServicesFactory.GetService<IKtTestService>().GetSubcategoryName(row.Id),
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
		public ActionResult Subscribe(Guid id)
		{
			if (!UserAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			ServicesFactory.GetService<IKtUsersService>().Subscribe(SessionWrapper.User.Username, id);

			_log.Info("Student subscribed for " + id, SessionWrapper.User.Username);

			return Json("Succesfully subscribed!", JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult Unsubscribe(Guid id)
		{
			if (!UserAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			ServicesFactory.GetService<IKtUsersService>().Unsubscribe(SessionWrapper.User.Username, id);

			_log.Info("Student unsubscribed for " + id, SessionWrapper.User.Username);

			return Json("Succesfully unsubscribed!", JsonRequestBehavior.AllowGet);
		}
	}
}
