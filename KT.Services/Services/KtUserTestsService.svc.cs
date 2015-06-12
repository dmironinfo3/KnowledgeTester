using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
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
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KtUserTestsService" in code, svc and config file together.
	public class KtUserTestsService : IKtUserTestsService
	{
		public void DoWork()
		{
		}

		public bool IsTestGenerated(Guid id, string username, out GeneratedTestDto test)
		{
			using (var db = new KTEntities())
			{
				var studentTest =
					db.GeneratedTests
					.Include("GeneratedQuestions")
					.Include("User")
					.DefaultIfEmpty(null).FirstOrDefault(a => a.TestId.Equals(id) && a.Username.Equals(username));

				var testIsGenerated = studentTest != null;

				if (testIsGenerated)
				{
					test = (new GeneratedTestsMapper()).Map(studentTest);
					return true;
				}
			}
			test = null;
			return false;
		}

		public GeneratedTestDto GenerateTest(Guid testId, string username)
		{
			using (var db = new KTEntities())
			{
				var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == testId);
				var student = db.Users.DefaultIfEmpty(null).FirstOrDefault(a => a.Username == username);

				if (test != null && student != null)
				{
					var subCatQuestions = db.Questions.Where(a => a.SubcategoryId == test.SubcategoryId).ToList();
					var selectedQuestions = new List<Question>();

					var rnd = new Random();
					for (int i = 0; i < 10; i++)
					{
						var valid = false;
						while (!valid)
						{
							var s = subCatQuestions[rnd.Next(subCatQuestions.Count)];

							if (!selectedQuestions.Select(a => a.Id).Contains(s.Id))
							{
								valid = true;
								selectedQuestions.Add(s);
							}
						}
					}

					var st = new GeneratedTest()
						{
							Id = Guid.NewGuid(),
							IsFinished = false,
							IsVerified = false,
							User = student,
							MaxScore = 10,
							Score = 0,
							Test = test,
							GeneratedQuestions = GetGeneratedQuestions(selectedQuestions)
						};

					db.GeneratedTests.AddObject(st);
					db.SaveChanges();
					return (new GeneratedTestsMapper()).Map(st);
				}
			}

			return null;
		}

		public void FinishTest(string username, Guid testId)
		{
			using (var db = new KTEntities())
			{
				var studentTest =
					db.GeneratedTests.DefaultIfEmpty(null).FirstOrDefault(a => a.Username == username && a.TestId == testId);

				if (studentTest != null)
				{
					int score = 0;
					foreach (var question in studentTest.GeneratedQuestions)
					{
						int add = 1;
						foreach (var answer in question.GeneratedAnswers)
						{
							if ((answer.IsSelected && !answer.Answer.IsCorrect) ||
								(answer.Answer.IsCorrect && !answer.IsSelected))
							{
								add = 0;
							}
						}
						score += add;
					}

					studentTest.IsFinished = true;
					studentTest.Score = score;
					db.SaveChanges();
				}
			}
		}

		public bool IsValidInProgress(Guid testId, string username)
		{
			using (var db = new KTEntities())
			{
				var test = db.GeneratedTests.DefaultIfEmpty(null)
					.FirstOrDefault(a => a.TestId == testId && username == a.Username);

				if (test == null)
				{
					//the test wasn't generated yet
					return true;
				}

				//the test is ongoing, need to see if it's finished
				var ongoing = test.Test.StartDate <= DateTime.Now &&
							  test.Test.EndDate >= DateTime.Now;

				if (ongoing)
				{
					if (!test.IsFinished)
					{
						return true;
					}
				}
				return false;
			}
		}

		public int GetScore(Guid testId, string username)
		{
			using (var db = new KTEntities())
			{
				var tst = db.GeneratedTests.DefaultIfEmpty(null).FirstOrDefault(a => a.TestId == testId && a.Username == username);
				if (tst != null)
					return tst.Score;
				return 0;
			}
		}

		private EntityCollection<GeneratedQuestion> GetGeneratedQuestions(IEnumerable<Question> selectedQuestions)
		{
			var collection = new EntityCollection<GeneratedQuestion>();
			foreach (var selectedQuestion in selectedQuestions)
			{
				collection.Add(new GeneratedQuestion()
				{
					Id = new Guid(),
					Question = selectedQuestion,
					ArgumentText = selectedQuestion.CorrectArgument,
					GeneratedAnswers = GetGeneratedAnswers(selectedQuestion.Answers)
				});
			}
			return collection;
		}

		private EntityCollection<GeneratedAnswer> GetGeneratedAnswers(IEnumerable<Answer> entityCollection)
		{
			var collection = new EntityCollection<GeneratedAnswer>();
			foreach (var answer in entityCollection)
			{
				collection.Add(new GeneratedAnswer()
				{
					Id = new Guid(),
					IsSelected = false,
					AnswerId = answer.Id
				});
			}
			return collection;
		}

		private static int IsCorrect(IEnumerable<GeneratedAnswer> list, Question question)
		{
			var correctIds = question.Answers.Where(a => a.IsCorrect).Select(a => a.Id).ToList();
			var selectedIds = list.Where(a => a.IsSelected).Select(a => a.Id).ToList();

			if (correctIds.Count != selectedIds.Count)
			{
				return 0;
			}

			if (selectedIds.Any(selectedId => !correctIds.Contains(selectedId)))
			{
				return 0;
			}

			if (correctIds.Any(correctId => !selectedIds.Contains(correctId)))
			{
				return 0;
			}

			return 1;
		}
	}
}
