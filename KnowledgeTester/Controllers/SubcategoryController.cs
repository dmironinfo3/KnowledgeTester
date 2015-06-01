﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.DB;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;

namespace KnowledgeTester.Controllers
{
	public class SubcategoryController : Controller
	{
		//
		// GET: /Subcategory/

		private Guid _subcatId;

		public ActionResult Index(Guid? id)
		{
			// user rights
			if (!SessionWrapper.UserIsAdmin)
			{
				return RedirectToAction("Index", "Home");
			}

			if (id == null)
			{
				ViewBag.Message = "Please select a valid subcategory!";
				return RedirectToAction("Index", "Category", new { id = SessionWrapper.CurrentCategoryId });
			}
			var catName = KT.DB.Helpers.Categories.GetById(SessionWrapper.CurrentCategoryId).Name;
			if (id.Value.Equals(Guid.Empty))
			{
				var m = new SubcategoryModel(catName);
				return View(m);
			}

			var subCat = KT.DB.Helpers.Subcategories.GetById(id.Value);

			if (subCat != null)
			{
				var m = new SubcategoryModel(subCat, catName);
				_subcatId = m.Id;

				ViewBag.IsExistingSubcategory = true;
				SessionWrapper.CurrentSubcategoryId = subCat.Id;

				return View(m);
			}

			ViewBag.Message = "Category does not exists!";

			return View();
		}

		[HttpPost]
		public ActionResult Save(SubcategoryModel model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Index", "Subcategory", new { id = model.Id });
			}

			_subcatId = model.Id.Equals(Guid.Empty) ?
				KT.DB.Helpers.Subcategories.Save(model.Name, SessionWrapper.CurrentCategoryId) :
				KT.DB.Helpers.Subcategories.Save(model.Name, SessionWrapper.CurrentCategoryId, model.Id);

			return RedirectToAction("Index", "Subcategory", new { id = _subcatId });
		}

		[HttpPost]
		public ActionResult DeleteQuestion(Guid id)
		{
			KT.DB.Helpers.Questions.Delete(id);

			return Json("Question is deleted!", JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetQuestions()
		{
			var s = new List<Question>();
			if (!SessionWrapper.CurrentSubcategoryId.Equals(Guid.Empty))
			{
				s = KT.DB.Helpers.Questions.GetBySubcategory(SessionWrapper.CurrentSubcategoryId);
			}

			var result = new
			{
				total = (int)Math.Ceiling((double)s.Count / 10),
				page = 1,
				records = s.Count,
				rows = (from row in s
						orderby KT.DB.Helpers.Questions.GetUsability(row.Id) descending 
						select new
						{
							Id = row.Id,
							Text = row.Text.Substring(0, row.Text.Length < 25 ? row.Text.Length : 25) + (row.Text.Length < 25 ? string.Empty : "..."),
							Multiple = row.MultipleAnswer.ToString().ToLower(),
							Usability = KT.DB.Helpers.Questions.GetUsability(row.Id) + " %"
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}