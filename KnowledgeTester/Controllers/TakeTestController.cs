using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.DB.Objects;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;

namespace KnowledgeTester.Controllers
{
	public class TakeTestController : Controller
	{
		//
		// GET: /TakeTest/

		public ActionResult Index(Guid? id)
		{
			
			if (id == null)
			{
				ViewBag.Message = "Please select a valid test to take!";
				return RedirectToAction("Index", "StudentPanel");
			}

			if (!id.Value.Equals(Guid.Empty))
			{
				if (!KT.DB.Helpers.StudentTests.IsValidInProgress(id.Value, SessionWrapper.Student.Username))
				{
					ViewBag.Message = "Please select a valid test to take!";
					return RedirectToAction("Index", "StudentPanel");
				}

				OngoingTest test;
				var testGenerated = KT.DB.Helpers.StudentTests.IsTestGenerated(id.Value, SessionWrapper.Student.Username, out test);

				if (testGenerated)
				{
					return View(new TakeTestModel(test));
				}
				else
				{
					test = KT.DB.Helpers.StudentTests.GenerateTest(id.Value, SessionWrapper.Student.Username);
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
				var ans = CompressAnswer(q.Answers, q.Argument);

				KT.DB.Helpers.Answers.SaveTestAnswer(SessionWrapper.Student.Username, model.TestId, q.Id, ans);
			}

			KT.DB.Helpers.StudentTests.FinishTest(SessionWrapper.Student.Username, model.TestId);
		}

		private string CompressAnswer(IEnumerable<TakeAnswerModel> answers, string argument)
		{
			var takeAnswerModels = answers as IList<TakeAnswerModel> ?? answers.ToList();
			if(takeAnswerModels.Count(a => !a.IsSelected) == takeAnswerModels.Count)
			{
				return string.Empty;
			}

			var r = string.Empty;

			foreach (var ans in takeAnswerModels)
			{
				if (ans.IsSelected)
				{
					r += ans.Id.ToString() + ",";
				}
			}
			r = r.Substring(0, r.Length - 1);

			r += "|" + argument;

			return r;
		}

		public ActionResult SaveUnfinishedTest(TakeTestModel model)
		{
			return Json(true);
		}
	}
}
