using System;
using System.Collections.Generic;
using System.Linq;

namespace KT.DB.Helpers
{
	public class Tests
	{
		public static List<Test> GetAll()
		{
			using (var db = new KTEntities())
			{
				return db.Tests.Include("Subcategory").Include("Students").ToList();
			}
		}

		public static void Delete(Guid id)
		{
			using (var db = new KTEntities())
			{
				var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				db.Tests.DeleteObject(test);
				db.SaveChanges();
			}
		}

		public static Test GetById(Guid testId)
		{
			using (var db = new KTEntities())
			{
				var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == testId);
				return test;
			}
		}

		public static Guid Save(string name, DateTime? startDate, DateTime? endDate, int? duration, Guid subcategoryId, Guid? id = null)
		{
			using (var db = new KTEntities())
			{
				if (id.HasValue && !id.Equals(Guid.Empty))
				{
					var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

					if (test != null)
					{
						test.Name = name;
						if (startDate != null) test.StartDate = startDate.Value;
						test.EndDate = endDate;
						if (duration != null) test.MinutesDuration = duration.Value;
						test.SubcategoryId = subcategoryId;
					}
					db.SaveChanges();
					return test.Id;
				}
				else
				{
					var test = new Test { Name = name, Id = Guid.NewGuid(), SubcategoryId = subcategoryId, StartDate = startDate.Value, EndDate = endDate, MinutesDuration = duration.Value };
					db.Tests.AddObject(test);
					db.SaveChanges();
					return test.Id;

				}
			}
		}

		public static Object GenerateForUser(Guid testId, string username)
		{
			return null;
		}

		public static List<Test> GetAllUpcoming(string username)
		{
			using (var db = new KTEntities())
			{
				var tst = new List<Test>();

				var tests = db.Students.Include("Tests").First(a => a.Username == username).Tests.ToList();

				foreach (var test in tests)
				{
					if (StudentTests.IsValidInProgress(test.Id, username))
					{
						tst.Add(db.Tests.Include("Students").Include("Subcategory").First(a => a.Id == test.Id));
					}
				}
				return tst;
			}
		}

		public static IEnumerable<Test> GetFinishedTests(string username)
		{
			using (var db = new KTEntities())
			{
				var tests = db.StudentTests.Include("Test").Where(a => a.StudentUsername == username
				&& a.Finished.Value).ToList();

				foreach (var t in tests)
				{
					yield return db.Tests.Include("Subcategory").DefaultIfEmpty(null).FirstOrDefault(a => a.Id == t.TestId);
				}
			}
		}
	}
}
