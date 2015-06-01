using System;
using System.Linq;

namespace KT.DB.Helpers
{
	public class Answers
	{
		public static void AddEmpyFor(Guid id)
		{
			using (var db = new KTEntities())
			{
				var ans = new Answer()
					{
						Id = Guid.NewGuid(),
						Text = string.Empty,
						IsCorrect = false,
						QuestionId = id
					};

				db.Answers.AddObject(ans);
				db.SaveChanges();
			}
		}

		public static void Delete(Guid id)
		{
			using (var db = new KTEntities())
			{
				var ans = db.Answers.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				if (ans != null)
				{
					db.Answers.DeleteObject(ans);
					db.SaveChanges();
				}
			}
		}

		public static void Save(Guid id, string text, bool isCorrect)
		{
			using (var db = new KTEntities())
			{
				var ans = db.Answers.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				if (ans != null)
				{
					ans.Text = text;
					ans.IsCorrect = isCorrect;
					db.SaveChanges();
				}
			}
		}

		public static void SaveTestAnswer(string username, Guid testId, Guid questionId, string ans)
		{
			using (var db = new KTEntities())
			{
				var studentTest =
					db.StudentTests.DefaultIfEmpty(null).FirstOrDefault(a => a.StudentUsername == username && a.TestId == testId);

				if (studentTest != null)
				{
					if (questionId.Equals(studentTest.Q1Id))
					{
						studentTest.Q1Answer = ans;
						db.SaveChanges();
						return;
					}
					if (questionId.Equals(studentTest.Q2Id))
					{
						studentTest.Q2Answer = ans;
						db.SaveChanges();
						return;
					}
					if (questionId.Equals(studentTest.Q3Id))
					{
						studentTest.Q3Answer = ans;
						db.SaveChanges();
						return;
					}
					if (questionId.Equals(studentTest.Q4Id))
					{
						studentTest.Q4Answer = ans;
						db.SaveChanges();
						return;
					}
					if (questionId.Equals(studentTest.Q5Id))
					{
						studentTest.Q5Answer = ans;
						db.SaveChanges();
						return;
					}
					if (questionId.Equals(studentTest.Q6Id))
					{
						studentTest.Q6Answer = ans;
						db.SaveChanges();
						return;
					}
					if (questionId.Equals(studentTest.Q7Id))
					{
						studentTest.Q7Answer = ans;
						db.SaveChanges();
						return;
					}
					if (questionId.Equals(studentTest.Q8Id))
					{
						studentTest.Q8Answer = ans;
						db.SaveChanges();
						return;
					}
					if (questionId.Equals(studentTest.Q9Id))
					{
						studentTest.Q9Answer = ans;
						db.SaveChanges();
						return;
					}
					if (questionId.Equals(studentTest.Q10Id))
					{
						studentTest.Q10Answer = ans;
						db.SaveChanges();
						return;
					}
				}
			}
		}
	}
}
