using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;
using KT.ExcelImporter;

namespace KnowledgeTester.Controllers
{
	public class SubcategoryController : BaseController
	{
		//
		// GET: /Subcategory/

		private Guid _subcatId;

		public ActionResult Index(Guid? id)
		{
			// user rights
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}

			if (id == null)
			{
				ViewBag.Message = "Please select a valid subcategory!";
				return RedirectToAction("Index", "Category", new { id = SessionWrapper.CurrentCategoryId });
			}
			var catName = ServicesFactory.GetService<IKtCategoriesService>().GetById(SessionWrapper.CurrentCategoryId).Name;
			if (id.Value.Equals(Guid.Empty))
			{
				var m = new SubcategoryModel(catName);
				return View(m);
			}

			var subCat = ServicesFactory.GetService<IKtSubcategoriesService>().GetById(id.Value);

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
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			if (!ModelState.IsValid)
			{
				return View("Index", model);
			}

			_subcatId = model.Id.Equals(Guid.Empty) ?
				ServicesFactory.GetService<IKtSubcategoriesService>().Save(model.Name, SessionWrapper.CurrentCategoryId) :
				ServicesFactory.GetService<IKtSubcategoriesService>().Save(model.Name, SessionWrapper.CurrentCategoryId, model.Id);

			return RedirectToAction("Index", "Subcategory", new { id = _subcatId });
		}

		[HttpPost]
		public ActionResult DeleteQuestion(Guid id)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			ServicesFactory.GetService<IKtQuestionsService>().Delete(id);

			return Json("Question is deleted!", JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetQuestions(string text)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			var s = new List<QuestionDto>();
			if (!SessionWrapper.CurrentSubcategoryId.Equals(Guid.Empty))
			{
				s = ServicesFactory.GetService<IKtQuestionsService>().GetBySubcategory
				(SessionWrapper.CurrentSubcategoryId).ToList();
			}

			if (!String.IsNullOrEmpty(text))
			{
				s = s.Where(a => a.Text.Contains(text)).ToList();
			}
			var result = new
			{
				total = (int)Math.Ceiling((double)s.Count / 10),
				page = 1,
				records = s.Count,
				rows = (from row in s
						orderby ServicesFactory.GetService<IKtQuestionsService>().GetUsability(row.Id) descending
						select new
						{
							Id = row.Id,
							Text = row.Text.Substring(0, row.Text.Length < 75 ? row.Text.Length : 75) + (row.Text.Length < 75 ? string.Empty : "..."),
							Multiple = row.MultipleResponse.ToString().ToLower(),
							Usability = ServicesFactory.GetService<IKtQuestionsService>().GetUsability(row.Id) + " %"
						}).ToArray()
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult Upload(HttpPostedFileBase file)
		{
			if (!AdminAllowed)
			{
				return RedirectToAction("Index", "Home");
			}
			if (file != null)
			{
				if (file.ContentLength > 0)
				{
					var fileStream = new StreamReader(file.InputStream);

					var dtos = ServicesFactory.GetService<IExcelParser>().Parse(fileStream);

					foreach (var dto in dtos)
					{
						//save question
						var id = ServicesFactory.GetService<IKtQuestionsService>()
							.Save(dto.Text, SessionWrapper.CurrentSubcategoryId,
								null, dto.MultipleResponse, dto.Argument);

						foreach (var ans in dto.Answers)
						{
							//saving answers
							ServicesFactory.GetService<IKtAnswersService>().Save(ans.Id, id, ans.Text, ans.IsCorrect);
						}
					}
				}
			}

			return RedirectToAction("Index", "Subcategory", new { id = SessionWrapper.CurrentSubcategoryId });
		}

		public FileResult DownloadTemplate()
		{
			var bytearray = System.IO.File.ReadAllBytes(HttpContext.Server.MapPath("~/KT Template.xlsx"));
			return File(bytearray, "application/force-download", "KT Template.xlsx");
		}
	}
}
