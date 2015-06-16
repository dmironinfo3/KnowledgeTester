using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using KT.DB;
using KT.DB.CRUD;
using KT.DTOs.Objects;
using KT.EmailSender;
using KT.ServiceInterfaces;
using KT.Services.Mappers;

namespace KT.Services.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KtUserTestsService" in code, svc and config file together.
	public class KtUserTestsService : IKtUserTestsService
	{
		private static readonly ICrud<GeneratedTest> Repository = CrudFactory<GeneratedTest>.Get();

		public bool IsTestGenerated(Guid id, string username, out GeneratedTestDto test)
		{
			var studentTest = GetTest(id, username);

			var testIsGenerated = studentTest != null;

			if (testIsGenerated)
			{
				test = (new GeneratedTestsMapper()).Map(studentTest);
				return true;
			}

			test = null;
			return false;
		}

		public GeneratedTestDto GenerateTest(Guid testId, string username)
		{
			var userRepository = CrudFactory<User>.Get();
			var testRepository = CrudFactory<Test>.Get();
			var questionsRepository = CrudFactory<Question>.Get();

			var test = testRepository.Read(a => a.Id == testId);
			var student = userRepository.Read(a => a.Username == username);

			if (test != null && student != null)
			{
				var subCatQuestions = questionsRepository.ReadArray(a => a.SubcategoryId == test.SubcategoryId, new[] { "Answers" });
				var selectedQuestions = new List<Question>();

				var rnd = new Random();
				for (int i = 0; i < test.QuestionCount; i++)
				{
					var valid = false;
					while (!valid)
					{
						var s = subCatQuestions[rnd.Next(subCatQuestions.Length)];

						if (!selectedQuestions.Select(a => a.Id).Contains(s.Id))
						{
							valid = true;
							selectedQuestions.Add(s);
						}
					}
				}

				var generatedTest = new GeneratedTest
				{
					Id = Guid.NewGuid(),
					IsFinished = false,
					IsVerified = false,
					Username = student.Username,
					MaxScore = test.QuestionCount.Value,
					Score = 0,
					TestId = test.Id
				};

				generatedTest = Repository.Create(generatedTest);

				SaveQuestions(generatedTest.Id, selectedQuestions);

				return (new GeneratedTestsMapper()).Map(GetTest(testId, username));
			}

			return null;
		}

		public void FinishTest(string username, Guid testId)
		{
			var studentTest = GetTest(testId, username);

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
				Repository.Update(studentTest);
			}
		}

		private void SendEmail(string instruct, string username, Guid generatedTestId)
		{
			var user = CrudFactory<User>.Get().Read(a => a.Username == username);
			var test = CrudFactory<GeneratedTest>.Get().Read(a => a.Id == generatedTestId, new []{"Test"});
			var instructor = CrudFactory<User>.Get().Read(a => a.Username == instruct);

			var templateInfo = new EmailTemplateInfo()
			{
				LastName = user.LastName,
				FirstName = user.FirstName,
				MaxScore = test.MaxScore,
				Score = test.Score,
				ProfFirstName = instructor.FirstName,
				ProfLastName = instructor.LastName,
				TestName = test.Test.Name
			};

			var email = EmailBuilder.Build(user.Email, test.Test.Name, templateInfo);

			EmailBuilder.GetSender().Send(email);
		}

		public bool IsValidInProgress(Guid testId, string username)
		{
			var test = GetTest(testId, username);

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

		public int GetScore(Guid testId, string username)
		{
			var tst = GetTest(testId, username);
			if (tst != null)
				return tst.Score;
			return 0;
		}

		public TestRestultRowDto[] GetTestResultRows(Guid testId)
		{
			var relatedObjects = new[] { "User" };

			var tst = Repository.ReadArray(a => a.TestId == testId, relatedObjects);

			return tst.Select(t => new TestRestultRowDto()
			{
				Username = t.Username,
				FullName =
					String.Format("{0} {1} ({2})", t.User.FirstName, t.User.LastName, t.Username),
				Score = t.Score,
				Validated = t.IsVerified
			}).ToArray();
		}

		public TestRestultDto GetTestResults(Guid testId)
		{
			var testRepository = CrudFactory<Test>.Get();

			var tst = Repository.ReadArray(a => a.TestId == testId);
			var testName = testRepository.Read(a => a.Id == testId).Name;

			return new TestRestultDto
			{
				Subscriptions = tst.Count(),
				Name = testName,
				AverageScore = tst.Average(a => a.Score),
			};
		}

		public bool IsValidated(Guid id, string username)
		{
			var tst = Repository.Read(a => a.TestId == id && a.Username == username);

			return tst != null && tst.IsVerified;
		}

		public TestReviewDto GetTestReview(Guid id, string user)
		{
			var tst = GetTest(id, user);

			return (new TestReviewMapper()).Map(tst);
		}

		public void UpdateScore(Guid testId, string username, int score)
		{
			var tst = Repository.Read(a => a.TestId == testId && a.Username == username);

			if (tst != null)
			{
				tst.Score = score;
				Repository.Update(tst);
			}
		}

		public void Validate(string instructor, Guid testId, string username)
		{
			var tst = Repository.Read(a => a.TestId == testId && a.Username == username);

			if (tst != null)
			{
				tst.IsVerified = true;
				Repository.Update(tst);
				SendEmail(instructor, username, tst.Id);
			}
		}

		private void SaveQuestions(Guid generatedTestId, IEnumerable<Question> selectedQuestions)
		{
			var generatedQuestionRepository = CrudFactory<GeneratedQuestion>.Get();

			foreach (var selectedQuestion in selectedQuestions)
			{
				var generatedQuestion = new GeneratedQuestion
				{
					Id = Guid.NewGuid(),
					QuestionId = selectedQuestion.Id,
					ArgumentText = String.Empty,
					GeneratedTestId = generatedTestId
				};

				generatedQuestionRepository.Create(generatedQuestion);

				SaveAnswers(generatedQuestion.Id, selectedQuestion.Answers);
			}
		}

		private void SaveAnswers(Guid generatedQuestionId, IEnumerable<Answer> selectedAnswers)
		{
			var generatedAnswersRepository = CrudFactory<GeneratedAnswer>.Get();

			foreach (var selectedAnswer in selectedAnswers)
			{
				var ans = new GeneratedAnswer()
				{
					Id = Guid.NewGuid(),
					IsSelected = false,
					AnswerId = selectedAnswer.Id,
					GeneratedQuestionId = generatedQuestionId
				};

				generatedAnswersRepository.Create(ans);
			}
		}

		private GeneratedTest GetTest(Guid id, string username)
		{
			var relatedObjects = new[]
			{
				"GeneratedQuestions", "GeneratedQuestions.Question", "GeneratedQuestions.GeneratedAnswers",
				"GeneratedQuestions.GeneratedAnswers.Answer", "User", "Test"
			};

			var studentTest = Repository.Read(a => a.TestId.Equals(id) && a.Username.Equals(username), relatedObjects);

			return studentTest;
		}
	}
}
