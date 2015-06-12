using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using KT.DB;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;
using KT.Services.Mappers;

namespace KT.Services.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KtQuestionsService" in code, svc and config file together.
	public class KtQuestionsService : IKtQuestionsService
	{
		public int GetCountBySubcategory(Guid subCatId)
		{
			using (var db = new KTEntities())
			{
				var count = db.Questions.Count(a => a.SubcategoryId.Equals(subCatId));
				return count;
			}
		}

		public void Delete(Guid id)
		{
			using (var db = new KTEntities())
			{
				var q = db.Questions.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				db.Questions.DeleteObject(q);
				db.SaveChanges();
			}
		}

		public IEnumerable<QuestionDto> GetBySubcategory(Guid id)
		{
			using (var db = new KTEntities())
			{
				var all = db.Questions.Where(a => a.SubcategoryId == id);
				return (new QuestionsMapper()).Map(all);
			}
		}

		public double GetUsability(Guid id)
		{
			using (var db = new KTEntities())
			{
				var q = db.Questions.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				if (q != null)
				{
					var allSubcatTests = db.GeneratedQuestions.Count(a => a.GeneratedTest.Test.Subcategory.Id == q.SubcategoryId);

					var thisQuestionUsedIn = db.GeneratedQuestions.Count(a => a.Id==id);

					if (allSubcatTests == 0) return 0;
					double percentage = (0 / allSubcatTests) * 100;
					return Math.Round(percentage, 2);
				}
				return 0;
			}
		}

		public QuestionDto GetById(Guid id)
		{
			using (var db = new KTEntities())
			{
				var q = db.Questions.Include("Answers").DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);
				return (new QuestionsMapper()).Map(q);
			}
		}

		public Guid Save(string text, Guid subCatId, Guid? qId = null, bool isMultiple = false, string argument = "")
		{
			using (var db = new KTEntities())
			{
				if (qId.HasValue && !qId.Equals(Guid.Empty))
				{
					var q = db.Questions.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == qId);

					if (q != null)
					{
						q.Text = text;
						q.MultipleAnswer = isMultiple;
						q.CorrectArgument = argument;
						db.SaveChanges();
					}


					return qId.Value;
				}
				else
				{
					var q = new Question
					{
						Text = text,
						Id = Guid.NewGuid(),
						SubcategoryId = subCatId,
						CorrectArgument = argument,
						MultipleAnswer = isMultiple
					};
					db.Questions.AddObject(q);
					db.SaveChanges();
					return q.Id;
				}
			}
		}
	}
}
