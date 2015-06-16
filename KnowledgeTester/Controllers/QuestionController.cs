using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.ServiceInterfaces;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;

namespace KnowledgeTester.Controllers
{
	public class QuestionController : BaseController
	{
		//
		// GET: /Question/

		private Guid _qId;

		public ActionResult Index(Guid? id)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			// user rights
			

			if (TempData["ModelInvalid"] != null)
			{
				ViewBag.Message = TempData["ModelInvalid"];
			}

			if (id == null)
			{
				ViewBag.Message = "Please select a valid question!";
				return RedirectToAction("Index", "Subcategory", new { id = SessionWrapper.CurrentSubcategoryId });
			}

			var subCatName = ServicesFactory.GetService<IKtSubcategoriesService>().GetById(SessionWrapper.CurrentSubcategoryId).Name;

			if (id.Value.Equals(Guid.Empty))
			{
				var m = new QuestionModel(subCatName);
				return View(m);
			}

			var q = ServicesFactory.GetService<IKtQuestionsService>().GetById(id.Value);

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
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			if (!ModelState.IsValid)
			{
				TempData["ModelInvalid"] = "The save was not performed. Please review the page fileds and save again.";
				return RedirectToAction("Index", "Question", new { id = model.Id });
			}

			_qId = model.Id.Equals(Guid.Empty) ?
				ServicesFactory.GetService<IKtQuestionsService>().Save(model.Text, SessionWrapper.CurrentSubcategoryId) :
				ServicesFactory.GetService<IKtQuestionsService>().Save(model.Text, 
					SessionWrapper.CurrentSubcategoryId, model.Id, model.IsMultiple, model.CorrectArgument);

			if (model.Answers != null)
			{
				foreach (var ans in model.Answers)
				{
					ServicesFactory.GetService<IKtAnswersService>().Save(ans.Id, _qId, ans.Text, ans.IsCorrect);
				}
			}

			return RedirectToAction("Index", "Question", new { id = _qId });
		}

		[HttpPost]
		public ActionResult AddNewAnswer(Guid id)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			ServicesFactory.GetService<IKtAnswersService>().AddEmpyFor(id);
			return RedirectToAction("Index", "Question", new { id });
		}

		[HttpPost]
		public ActionResult DeleteAnswer(Guid id, Guid questionId)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			ServicesFactory.GetService<IKtAnswersService>().Delete(id);
			return RedirectToAction("Index", "Question", new { id = questionId });
		}
	}
}
