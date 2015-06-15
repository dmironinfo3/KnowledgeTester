using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using KT.DB;
using KT.DB.CRUD;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;
using KT.Services.Mappers;

namespace KT.Services.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KtQuestionsService" in code, svc and config file together.
	public class KtQuestionsService : IKtQuestionsService
	{
		private static readonly ICrud<Question> Repository = CrudFactory<Question>.Get();

		public int GetCountBySubcategory(Guid subCatId)
		{
			var count = Repository.ReadArray(a => a.SubcategoryId.Equals(subCatId)).Count();
			return count;
		}

		public void Delete(Guid id)
		{
			Repository.Delete(a => a.Id == id);
		}

		public QuestionDto[] GetBySubcategory(Guid id)
		{
			var relatedObjects = new[] { "Answers" };
			var all = Repository.ReadArray(a => a.SubcategoryId == id, relatedObjects);
			return (new QuestionsMapper()).Map(all).ToArray();
		}

		public double GetUsability(Guid id)
		{
			var testsRepository = CrudFactory<GeneratedTest>.Get();
			var questionsRepository = CrudFactory<GeneratedQuestion>.Get();

			var q = Repository.Read(a => a.Id == id);

			if (q != null)
			{
				var allSubcatTests = testsRepository.ReadArray(a => a.Test.Subcategory.Id == q.SubcategoryId).Count();

				var thisQuestionUsedIn = questionsRepository.ReadArray(a => a.QuestionId == id).Count();

				if (allSubcatTests == 0) return 0;
				double percentage = (thisQuestionUsedIn / allSubcatTests) * 100;
				return Math.Round(percentage, 2);
			}
			return 0;
		}

		public QuestionDto GetById(Guid id)
		{
			var relatedObjects = new[] { "Subcategory", "Answers" };
			var q = Repository.Read(a => a.Id == id, relatedObjects);
			return (new QuestionsMapper()).Map(q);
		}

		public Guid Save(string text, Guid subCatId, Guid? qId = null, bool isMultiple = false, string argument = "")
		{

			if (qId.HasValue && !qId.Equals(Guid.Empty))
			{
				var q = Repository.Read(a => a.Id == qId);

				if (q != null)
				{
					q.Text = text;
					q.MultipleAnswer = isMultiple;
					q.CorrectArgument = argument;

					Repository.Update(q);
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
				q = Repository.Create(q);
				return q.Id;
			}
		}

		public QuestionDto[] GetAll()
		{
			var relatedObjects = new[] { "Answers" };
			var all = Repository.ReadArray(a => true, relatedObjects).ToList();
			return (new QuestionsMapper()).Map(all).ToArray();
		}
	}
}
