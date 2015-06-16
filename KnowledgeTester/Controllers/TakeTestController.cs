using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;

namespace KnowledgeTester.Controllers
{
	public class TakeTestController : BaseController
	{
		//
		// GET: /TakeTest/

		public ActionResult Index(Guid? id)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			
			if (id == null)
			{
				ViewBag.Message = "Please select a valid test to take!";
				return RedirectToAction("Index", "StudentPanel");
			}

			if (!id.Value.Equals(Guid.Empty))
			{
				if (!ServicesFactory.GetService<IKtUserTestsService>().IsValidInProgress(id.Value, SessionWrapper.User.Username))
				{
					ViewBag.Message = "Please select a valid test to take!";
					return RedirectToAction("Index", "StudentPanel");
				}

				GeneratedTestDto test;
				var testGenerated = ServicesFactory.GetService<IKtUserTestsService>().IsTestGenerated(id.Value, SessionWrapper.User.Username, out test);

				if (testGenerated)
				{
					return View(new TakeTestModel(test));
				}
				else
				{
					test = ServicesFactory.GetService<IKtUserTestsService>().GenerateTest(id.Value, SessionWrapper.User.Username);
					return View(new TakeTestModel(test));
				}
			}

			ViewBag.Message = "Test does not exists!";

			return View();
		}

		[HttpPost]
		public ActionResult SubmitTest(TakeTestModel model)
		{
			if (model.TimeElapsed)
			{
				FinishTest(model);
			}
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Index", "TakeTest", new { id = model.TestId });
			}

			FinishTest(model);

			return RedirectToAction("Index", "StudentPanel");
		}

		private void FinishTest(TakeTestModel model)
		{
			foreach (var q in model.Questions)
			{
				foreach (var ans in q.Answers)
				{
					ServicesFactory.GetService<IKtAnswersService>().SaveTestAnswer(ans.Id, ans.IsSelected);
				}
			}

			ServicesFactory.GetService<IKtUserTestsService>().FinishTest(model.Username, model.TestId);
		}

	}
}
