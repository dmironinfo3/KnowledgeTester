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

			var cat = ServicesFactory.GetService<IKtCategoriesService>().GetById(id.Value);

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
			if (!ModelState.IsValid && SessionWrapper.User != null)
			{
				return RedirectToAction("Index", "Category", new { id = model.Id });
			}

			_catId = model.Id.Equals(Guid.Empty) ?
				ServicesFactory.GetService<IKtCategoriesService>().Save(SessionWrapper.User.Username, model.Name) :
				ServicesFactory.GetService<IKtCategoriesService>().Save(SessionWrapper.User.Username, model.Name, model.Id);

			return RedirectToAction("Index", "Category", new { id = _catId });
		}

		public ActionResult GetSubcategories(string name)
		{
			var s = new List<SubcategoryDto>();
			if (!SessionWrapper.CurrentCategoryId.Equals(Guid.Empty))
			{
				s = ServicesFactory.GetService<IKtSubcategoriesService>().GetAll().Where(a => a.CategoryId ==
				SessionWrapper.CurrentCategoryId).ToList();
			}


			if (!String.IsNullOrEmpty(name))
			{
				s = s.Where(a => a.Name.Contains(name)).ToList();
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
							Questions = ServicesFactory.GetService<IKtQuestionsService>().GetCountBySubcategory(row.Id)
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult DeleteSubcategory(Guid id)
		{
			ServicesFactory.GetService<IKtSubcategoriesService>().Delete(id);

			return Json("Subcategory is deleted!", JsonRequestBehavior.AllowGet);
		}
	}
}
