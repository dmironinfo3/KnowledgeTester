using System;
using System.Collections.Generic;
using System.Linq;

namespace KT.DB.Helpers
{
	public class Questions
	{
		public static int GetCountBySubcategory(Guid subCatId)
		{
			using (var db = new KTEntities())
			{
				var count = db.Questions.Count(a => a.SubcategoryId.Equals(subCatId));
				return count;
			}
		}

		public static void Delete(Guid id)
		{
			using (var db = new KTEntities())
			{
				var q = db.Questions.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				db.Questions.DeleteObject(q);
				db.SaveChanges();
			}
		}

		public static List<Question> GetBySubcategory(Guid id)
		{
			using (var db = new KTEntities())
			{
				var all = db.Questions.Where(a => a.SubcategoryId == id);
				return all.ToList();
			}
		}

		public static double GetUsability(Guid id)
		{
			using (var db = new KTEntities())
			{
				var q = db.Questions.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				if (q != null)
				{
					var allSubcatTests = db.StudentTests.Count(a => a.Test.Subcategory.Id == q.SubcategoryId);

					var thisQuestionUsedIn = db.StudentTests.Count(a => a.Q1Id == q.Id ||
																		a.Q2Id == q.Id ||
																		a.Q3Id == q.Id ||
																		a.Q4Id == q.Id ||
																		a.Q5Id == q.Id ||
																		a.Q6Id == q.Id ||
																		a.Q7Id == q.Id ||
																		a.Q8Id == q.Id ||
																		a.Q9Id == q.Id ||
																		a.Q10Id == q.Id);

					if (allSubcatTests == 0) return 0;
					double percentage = (thisQuestionUsedIn / allSubcatTests) * 100;
					return Math.Round(percentage, 2);
				}
				return 0;
			}
		}

		public static Question GetById(Guid id)
		{
			using (var db = new KTEntities())
			{
				var q = db.Questions.Include("Answers").DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);
				return q;
			}
		}

		public static Guid Save(string text, Guid subCatId, Guid? qId = null, bool isMultiple = false, string argument = "")
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
