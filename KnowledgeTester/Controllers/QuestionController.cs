using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;

namespace KnowledgeTester.Controllers
{
	public class QuestionController : Controller
	{
		//
		// GET: /Question/

		private Guid _qId;

		public ActionResult Index(Guid? id)
		{
			// user rights
			if (!SessionWrapper.UserIsAdmin)
			{
				return RedirectToAction("Index", "Home");
			}

			if (TempData["ModelInvalid"] != null)
			{
				ViewBag.Message = TempData["ModelInvalid"];
			}

			if (id == null)
			{
				ViewBag.Message = "Please select a valid question!";
				return RedirectToAction("Index", "Subcategory", new { id = SessionWrapper.CurrentSubcategoryId });
			}

			var subCatName = KT.DB.Helpers.Subcategories.GetById(SessionWrapper.CurrentSubcategoryId).Name;

			if (id.Value.Equals(Guid.Empty))
			{
				var m = new QuestionModel(subCatName);
				return View(m);
			}

			var q = KT.DB.Helpers.Questions.GetById(id.Value);

			if (q != null)
			{
				var m = new QuestionModel(q, subCatName);
				_qId = m.Id;
				ViewBag.IsExistingQuestion = true;

				return View(m);
			}

			ViewBag.Message = "Question does not exists!";

			return View();
		}

		[HttpPost]
		public ActionResult Save(QuestionModel model)
		{
			if (!ModelState.IsValid)
			{
				TempData["ModelInvalid"] = "The save was not performed. Please review the page fileds and save again.";
				return RedirectToAction("Index", "Question", new { id = model.Id });
			}

			_qId = model.Id.Equals(Guid.Empty) ?
				KT.DB.Helpers.Questions.Save(model.Text, SessionWrapper.CurrentSubcategoryId) :
				KT.DB.Helpers.Questions.Save(model.Text, SessionWrapper.CurrentSubcategoryId, model.Id, model.IsMultiple, model.CorrectArgument);

			if (model.Answers != null)
			{
				foreach (var ans in model.Answers)
				{
					KT.DB.Helpers.Answers.Save(ans.Id, ans.Text, ans.IsCorrect);
				}
			}

			return RedirectToAction("Index", "Question", new { id = _qId });
		}

		[HttpPost]
		public ActionResult AddNewAnswer(Guid id)
		{
			KT.DB.Helpers.Answers.AddEmpyFor(id);
			return RedirectToAction("Index", "Question", new { id });
		}

		[HttpPost]
		public ActionResult DeleteAnswer(Guid id, Guid questionId)
		{
			KT.DB.Helpers.Answers.Delete(id);
			return RedirectToAction("Index", "Question", new { id = questionId });
		}
	}
}
