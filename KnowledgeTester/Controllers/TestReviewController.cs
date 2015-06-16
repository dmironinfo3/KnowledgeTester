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
	public class TestReviewController : BaseController
	{
		//
		// GET: /TestReview/

		public ActionResult Index(Guid? id, string user)
		{
			// user rights
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}

			if (id == null || id == Guid.Empty || String.IsNullOrEmpty(user))
			{
				ViewBag.Message = "Please select a valid test!";
				return RedirectToAction("Index", "AdminPanel");
			}

			var dto = ServicesFactory.GetService<IKtUserTestsService>().GetTestReview(id.Value, user);

			var model = new TestReviewModel(dto)
			{
				Username = user,
				TestId = id.Value
			};


			return View(model);
		}

		public ActionResult Validate(Guid testId, string username, int? score)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			if (score != null)
			{
				ServicesFactory.GetService<IKtUserTestsService>().UpdateScore(testId, username, score.Value);
			}

			ServicesFactory.GetService<IKtUserTestsService>().Validate(SessionWrapper.User.Username, testId, username);

			return RedirectToAction("Index", "TestResults", new { id = SessionWrapper.CurrentTestResultId });
		}
	}
}
