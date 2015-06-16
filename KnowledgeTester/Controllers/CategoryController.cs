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
using KT.Logger;

namespace KnowledgeTester.Controllers
{
	public class CategoryController : BaseController
	{
		//
		// GET: /Category/

		private Guid _catId;
		private static ILogger _log = ServicesFactory.GetService<ILogger>();

		public ActionResult Index(Guid? id)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			try
			{
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
			catch (Exception ex)
			{
				_log.Error(ex.Message, SessionWrapper.User.Username, ex);
				throw;
			}
		}

		[HttpPost]
		public ActionResult Save(CategoryModel model)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			try
			{
				if (!ModelState.IsValid && SessionWrapper.User != null)
				{
					return RedirectToAction("Index", "Category", new { id = model.Id });
				}

				_catId = model.Id.Equals(Guid.Empty) ?
					ServicesFactory.GetService<IKtCategoriesService>().Save(SessionWrapper.User.Username, model.Name) :
					ServicesFactory.GetService<IKtCategoriesService>().Save(SessionWrapper.User.Username, model.Name, model.Id);

				_log.Info("New category added: " + model.Name, SessionWrapper.User.Username);
				return RedirectToAction("Index", "Category", new { id = _catId });
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, SessionWrapper.User.Username, ex);
				throw;
			}

		}

		public ActionResult GetSubcategories(string name)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			try
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
			catch (Exception ex)
			{
				_log.Error(ex.Message, SessionWrapper.User.Username, ex);
				throw;
			}
		}

		[HttpPost]
		public ActionResult DeleteSubcategory(Guid id)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			ServicesFactory.GetService<IKtSubcategoriesService>().Delete(id);

			_log.Info("Subcategory deleted: " + id, SessionWrapper.User.Username);

			return Json("Subcategory is deleted!", JsonRequestBehavior.AllowGet);
		}
	}
}
