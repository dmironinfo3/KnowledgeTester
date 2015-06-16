using System;
using System.Linq;

namespace KT.DB.CRUD
{
	public class GeneratedTestsRepository : ICrud<GeneratedTest>
	{
		public GeneratedTest Create(GeneratedTest entity)
		{
			using (var db = new KTEntities())
			{
				db.GeneratedTests.AddObject(entity);
				db.SaveChanges();
				return entity;
			}
		}

		public GeneratedTest Read(Predicate<GeneratedTest> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var test = db.GeneratedTests.Include(relatedObjects).AsEnumerable();

				if (test.Any())
				{
					return test.DefaultIfEmpty(null).FirstOrDefault(a => predicate(a));
				}

				return null;
			}
		}

		public GeneratedTest[] ReadArray(Predicate<GeneratedTest> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var test = db.GeneratedTests.Include(relatedObjects).AsEnumerable();
				if (test.Any())
				{
					return test.Where(a => predicate(a)).ToArray();
				}
				return new GeneratedTest[] { };
			}
		}

		public GeneratedTest Update(GeneratedTest entity)
		{
			using (var db = new KTEntities())
			{
				var val = db.GeneratedTests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == entity.Id);

				if (val != null)
				{
					val.IsFinished = entity.IsFinished;
					val.IsVerified = entity.IsVerified;
					val.MaxScore = entity.MaxScore;
					val.Score = entity.Score;
					db.SaveChanges();
				}
				return val;
			}
		}

		public void Delete(Predicate<GeneratedTest> predicate)
		{
			using (var db = new KTEntities())
			{
				var tests = db.GeneratedTests.AsEnumerable().Where(a => predicate(a));

				foreach (var test in tests)
				{
					db.GeneratedTests.DeleteObject(test);
				}
				db.SaveChanges();
			}
		}
	}
}
