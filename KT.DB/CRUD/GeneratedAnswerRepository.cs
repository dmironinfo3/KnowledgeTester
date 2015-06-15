using System;
using System.Linq;

namespace KT.DB.CRUD
{
	public class GeneratedAnswersRepository: ICrud<GeneratedAnswer>
	{
		public GeneratedAnswer Create(GeneratedAnswer entity)
		{
			using (var db = new KTEntities())
			{
				db.GeneratedAnswers.AddObject(entity);
				db.SaveChanges();
				return entity;
			}
		}

		public GeneratedAnswer Read(Predicate<GeneratedAnswer> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var test = db.GeneratedAnswers.Include(relatedObjects).DefaultIfEmpty(null).FirstOrDefault(a => predicate(a));
				return test;
			}
		}

		public GeneratedAnswer[] ReadArray(Predicate<GeneratedAnswer> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var test = db.GeneratedAnswers.Include(relatedObjects).Where(a => predicate(a));
				return test.ToArray();
			}
		}

		public GeneratedAnswer Update(GeneratedAnswer entity)
		{
			using (var db = new KTEntities())
			{
				var val = db.GeneratedAnswers.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == entity.Id);

				if (val != null)
				{
					val.GeneratedQuestionId = entity.GeneratedQuestionId;
					val.AnswerId = entity.AnswerId;
					val.IsSelected = entity.IsSelected;
					db.SaveChanges();
				}

				return val;
			}
		}

		public void Delete(Predicate<GeneratedAnswer> predicate)
		{
			throw new Exception("Generated Answer cannot be deleted!");
		}
	}
}
