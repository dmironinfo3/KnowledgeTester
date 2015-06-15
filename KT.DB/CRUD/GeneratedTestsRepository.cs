using System;
using System.Linq;

namespace KT.DB.CRUD
{
	public class GeneratedTestsRepository: ICrud<GeneratedTest>
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
				var test = db.GeneratedTests.Include(relatedObjects).DefaultIfEmpty(null).FirstOrDefault(a => predicate(a));
				return test;
			}
		}

		public GeneratedTest[] ReadArray(Predicate<GeneratedTest> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var test = db.GeneratedTests.Include(relatedObjects).Where(a => predicate(a));
				return test.ToArray();
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
			throw new Exception("Generated test cannot be deleted!");
		}
	}
}
