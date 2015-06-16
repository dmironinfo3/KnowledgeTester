using System;
using System.Linq;

namespace KT.DB.CRUD
{
	public class TestRepository : ICrud<Test>
	{
		public Test Create(Test entity)
		{
			using (var db = new KTEntities())
			{
				db.Tests.AddObject(entity);
				db.SaveChanges();
				return entity;
			}
		}

		public Test Read(Predicate<Test> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var test = db.Tests.Include(relatedObjects).AsEnumerable().DefaultIfEmpty(null).FirstOrDefault(a => predicate(a));
				return test;
			}
		}

		public Test[] ReadArray(Predicate<Test> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var test = db.Tests.Include(relatedObjects).AsEnumerable().Where(a => predicate(a));
				return test.ToArray();
			}
		}

		public Test Update(Test entity)
		{
			using (var db = new KTEntities())
			{
				var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == entity.Id);

				if (test != null)
				{
					test.Name = entity.Name;
					test.QuestionCount = entity.QuestionCount;
					test.SubcategoryId = entity.SubcategoryId;
					test.EndDate = entity.EndDate;
					test.MinutesDuration = entity.MinutesDuration;
					test.StartDate = entity.StartDate;

					db.SaveChanges();
				}

				return test;
			}
		}

		public void Delete(Predicate<Test> predicate)
		{
			using (var db = new KTEntities())
			{
				var tests = db.Tests.AsEnumerable().Where(a => predicate(a));

				foreach (var test in tests)
				{
					db.Tests.DeleteObject(test);
				}
				db.SaveChanges();
			}
		}
	}
}
