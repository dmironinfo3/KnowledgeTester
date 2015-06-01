using System;
using System.Collections.Generic;
using KT.DB.Objects;
using System.Linq;


namespace KT.DB.Helpers
{
	public class StudentTests
	{
		public static bool IsTestGenerated(Guid id, string username, out OngoingTest test)
		{
			using (var db = new KTEntities())
			{
				var studentTests =
					db.StudentTests
						.Include("Question")
						.Include("Question1")
						.Include("Question2")
						.Include("Question3")
						.Include("Question4")
						.Include("Question5")
						.Include("Question6")
						.Include("Question7")
						.Include("Question8")
						.Include("Question9")
					.DefaultIfEmpty(null).FirstOrDefault(a => a.TestId.Equals(id) && a.StudentUsername.Equals(username));

				var testIsGenerated = studentTests != null;

				if (testIsGenerated)
				{
					test = new OngoingTest(studentTests);
					return true;
				}
			}
			test = null;
			return false;
		}

		public static OngoingTest GenerateTest(Guid testId, string username)
		{
			using (var db = new KTEntities())
			{
				var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == testId);
				var student = db.Students.DefaultIfEmpty(null).FirstOrDefault(a => a.Username == username);

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

					//test
					for (int i = 0; i < 10; i++)
					{
						var ccctest = selectedQuestions.Count(a => a.Id == selectedQuestions[i].Id);
						if (ccctest != 1)
						{

						}
					}

					#region Create Test
					var st = new StudentTest()
						{
							Id = Guid.NewGuid(),
							StudentUsername = username,
							TestId = testId,
							Q1Id = selectedQuestions[0].Id,
							Q1Answer = String.Empty,
							Q2Id = selectedQuestions[1].Id,
							Q2Answer = String.Empty,
							Q3Id = selectedQuestions[2].Id,
							Q3Answer = String.Empty,
							Q4Id = selectedQuestions[3].Id,
							Q4Answer = String.Empty,
							Q5Id = selectedQuestions[4].Id,
							Q5Answer = String.Empty,
							Q6Id = selectedQuestions[5].Id,
							Q6Answer = String.Empty,
							Q7Id = selectedQuestions[6].Id,
							Q7Answer = String.Empty,
							Q8Id = selectedQuestions[7].Id,
							Q8Answer = String.Empty,
							Q9Id = selectedQuestions[8].Id,
							Q9Answer = String.Empty,
							Q10Id = selectedQuestions[9].Id,
							Q10Answer = String.Empty,

						};
					#endregion

					db.StudentTests.AddObject(st);
					db.SaveChanges();

					return new OngoingTest(st);
				}
			}
			return null;
		}

		public static void FinishTest(string username, Guid testId)
		{
			using (var db = new KTEntities())
			{
				var studentTest =
					db.StudentTests.Include("Question")
						.Include("Question1")
						.Include("Question2")
						.Include("Question3")
						.Include("Question4")
						.Include("Question5")
						.Include("Question6")
						.Include("Question7")
						.Include("Question8")
						.Include("Question9")
						.DefaultIfEmpty(null).FirstOrDefault(a => a.StudentUsername == username && a.TestId == testId);

				if (studentTest != null)
				{
					var score = IsCorrect(Answers(studentTest.Q1Answer, studentTest.Question), studentTest.Question) +
						IsCorrect(Answers(studentTest.Q2Answer, studentTest.Question1), studentTest.Question1) +
						IsCorrect(Answers(studentTest.Q3Answer, studentTest.Question2), studentTest.Question2) +
						IsCorrect(Answers(studentTest.Q4Answer, studentTest.Question3), studentTest.Question3) +
						IsCorrect(Answers(studentTest.Q5Answer, studentTest.Question4), studentTest.Question4) +
						IsCorrect(Answers(studentTest.Q6Answer, studentTest.Question5), studentTest.Question5) +
						IsCorrect(Answers(studentTest.Q7Answer, studentTest.Question6), studentTest.Question6) +
						IsCorrect(Answers(studentTest.Q8Answer, studentTest.Question7), studentTest.Question7) +
						IsCorrect(Answers(studentTest.Q9Answer, studentTest.Question8), studentTest.Question8) +
						IsCorrect(Answers(studentTest.Q10Answer, studentTest.Question9), studentTest.Question9);

					studentTest.Finished = true;
					studentTest.Score = score;
					db.SaveChanges();
				}
			}
		}

		private static int IsCorrect(IEnumerable<OngoingAnswer> list, Question question)
		{
			var correctIds = question.Answers.Where(a => a.IsCorrect).Select(a=>a.Id).ToList();
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

		private static List<OngoingAnswer> Answers(string p, Question question)
		{
			var spl = p.Split('|');
			var selected = new List<Guid>();
			if (spl.Length == 2)
			{
				selected = spl[0].Split(',').Select(a => (new Guid(a))).ToList();
			}

			return question.Answers.Select(ans =>
			new OngoingAnswer
			{
				Id = ans.Id,
				IsSelected = selected.Contains(ans.Id),
				Text = ans.Text
			}).ToList();
		}

		public static bool IsValidInProgress(Guid testId, string username)
		{
			using (var db = new KTEntities())
			{
				var test = db.StudentTests.DefaultIfEmpty(null)
					.FirstOrDefault(a => a.TestId == testId && username == a.StudentUsername);

				if (test == null)
				{
					//the test wasn't generated yet
					return true;
				}

				//the test is ongoing, need to see if it's finished
				var ongoing = test.Test.StartDate <= DateTime.Now &&
							  (test.Test.EndDate >= DateTime.Now || test.Test.StartDate.AddMinutes(2 * test.Test.MinutesDuration) >= DateTime.Now);

				if (ongoing)
				{
					if (!test.Finished.HasValue)
					{
						return true;
					}
					if (!test.Finished.Value)
					{
						return true;
					}
				}
				return false;
			}
		}

		public static int GetScore(Guid testId, string username)
		{
			using (var db = new KTEntities())
			{
				var tst = db.StudentTests.DefaultIfEmpty(null).FirstOrDefault(a => a.TestId == testId && a.StudentUsername == username);
				if (tst != null && tst.Score.HasValue)
					return tst.Score.Value;
				return 0;
			}
		}
	}
}
