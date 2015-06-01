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
	public class CategoryController : Controller
	{
		//
		// GET: /Category/

		private Guid _catId;

		public ActionResult Index(Guid? id)
		{
			// user rights
			if (!SessionWrapper.UserIsAdmin)
			{
				return RedirectToAction("Index", "Home");
			}

			if (id == null)
			{
				ViewBag.Message = "Please select a valid category!";
				return RedirectToAction("Index", "AdminPanel");
			}

			if (id.Value.Equals(Guid.Empty))
			{
				var m = new CategoryModel();
				return View(m);
			}

			var cat = KT.DB.Helpers.Categories.GetById(id.Value);

			if (cat != null)
			{
				var m = new CategoryModel(cat);
				_catId = m.Id;

				ViewBag.IsExistingCategory = true;
				SessionWrapper.CurrentCategoryId = cat.Id;

				return View(m);
			}

			ViewBag.Message = "Category does not exists!";

			return View();
		}

		[HttpPost]
		public ActionResult Save(CategoryModel model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Index", "Category", new { id = model.Id });
			}

			_catId = model.Id.Equals(Guid.Empty) ?
				KT.DB.Helpers.Categories.Save(model.Name) :
				KT.DB.Helpers.Categories.Save(model.Name, model.Id);

			return RedirectToAction("Index", "Category", new { id = _catId });
		}

		public ActionResult GetSubcategories()
		{
			var s = new List<Subcategory>();
			if (!SessionWrapper.CurrentCategoryId.Equals(Guid.Empty))
			{
				s = KT.DB.Helpers.Subcategories.GetByCategory(SessionWrapper.CurrentCategoryId);
			}

			var result = new
			{
				total = (int)Math.Ceiling((double)s.Count / 10),
				page = 1,
				records = s.Count,
				rows = (from row in s
						orderby row.Name ascending
						select new
						{
							Id = row.Id,
							Name = row.Name,
							Questions = KT.DB.Helpers.Questions.GetCountBySubcategory(row.Id)
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult DeleteSubcategory(Guid id)
		{
			KT.DB.Helpers.Subcategories.Delete(id);

			return Json("Subcategory is deleted!", JsonRequestBehavior.AllowGet);
		}
	}
}