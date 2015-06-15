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
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KtTestService" in code, svc and config file together.
	public class KtTestService : IKtTestService
	{
		private static readonly ICrud<Test> Repository = CrudFactory<Test>.Get();

		public TestDto[] GetAll()
		{
			var relatedObjects = new[] { "Subcategory", "Users" };
			return (new TestsMapper()).Map(Repository.ReadArray(a => true, relatedObjects)).ToArray();
		}

		public void Delete(Guid id)
		{
			Repository.Delete(a => a.Id == id);
		}

		public TestDto GetById(Guid testId)
		{
			var relatedObjects = new[] { "Subcategory", "Users" };
			var test = Repository.Read(a => a.Id == testId, relatedObjects);
			return (new TestsMapper()).Map(test);
		}

		public Guid Save(string name, DateTime? startDate, DateTime? endDate, int? duration, Guid subcategoryId, int? questions, Guid? id = null)
		{
			if (id.HasValue && !id.Equals(Guid.Empty))
			{
				var test = Repository.Read(a => a.Id == id);

				if (test != null)
				{
					test.Name = name;
					test.SubcategoryId = subcategoryId;

					if (startDate != null)
						test.StartDate = startDate.Value;
					if (endDate != null)
						test.EndDate = endDate.Value;
					if (duration != null)
						test.MinutesDuration = duration.Value;
					if (questions != null)
						test.QuestionCount = questions.Value;

					Repository.Update(test);
				}
				return id.Value;
			}
			else
			{
				var test = new Test
				{
					Name = name,
					Id = Guid.NewGuid(),
					SubcategoryId = subcategoryId,
					StartDate = startDate.Value,
					EndDate = endDate.Value,
					MinutesDuration = duration.Value,
					QuestionCount = questions
				};
				test = Repository.Create(test);
				return test.Id;
			}
		}

		public TestDto[] GetAllUpcoming(string username)
		{
			var relatedObjects = new[] { "Users", "Subcategory" };

			var usersRepository = CrudFactory<User>.Get();
			var usersRelatedObjects = new[] { "Tests" };

			var tests = usersRepository.Read(a => a.Username == username, usersRelatedObjects).Tests.ToList();

			var tst = (from test
					in tests
					   where (new KtUserTestsService()).IsValidInProgress(test.Id, username)
					   select Repository.Read(a => a.Id == test.Id, relatedObjects)).ToList();

			return (new TestsMapper()).Map(tst).ToArray();
		}

		public TestDto[] GetFinishedTests(string username)
		{
			var relatedObjects = new[] { "Test", "User", "GeneratedQuestions" };
			var generatedTestRepository = CrudFactory<GeneratedTest>.Get();

			var tests = generatedTestRepository.ReadArray(a => a.Username == username
				&& a.IsFinished, relatedObjects).ToList();

			return (new TestsMapper()).Map(tests.Select(a => a.Test)).ToArray();
		}

		public string GetSubcategoryName(Guid id)
		{
			var test = GetById(id);

			return (new KtSubcategoriesService()).GetById(test.SubcategoryId).Name;
		}

		public TestDto[] GetAllOtherThan(string username)
		{
			var all = GetAll().Where(a =>
				!GetAllUpcoming(username).Select(b => b.Id).Contains(a.Id) &&
					  !GetFinishedTests(username).Select(b => b.Id).Contains(a.Id));
			return all.ToArray();
		}
	}
}
