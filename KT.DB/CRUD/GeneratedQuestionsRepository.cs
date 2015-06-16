using System;
using System.Linq;

namespace KT.DB.CRUD
{
	public class GeneratedQuestionsRepository: ICrud<GeneratedQuestion>
	{
		public GeneratedQuestion Create(GeneratedQuestion entity)
		{
			using (var db = new KTEntities())
			{
				db.GeneratedQuestions.AddObject(entity);
				db.SaveChanges();
				return entity;
			}
		}

		public GeneratedQuestion Read(Predicate<GeneratedQuestion> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var val = db.GeneratedQuestions.Include(relatedObjects).AsEnumerable().DefaultIfEmpty(null).FirstOrDefault(a => predicate(a));
				return val;
			}
		}

		public GeneratedQuestion[] ReadArray(Predicate<GeneratedQuestion> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var val = db.GeneratedQuestions.Include(relatedObjects).AsEnumerable().Where(a => predicate(a));
				return val.ToArray();
			}
		}

		public GeneratedQuestion Update(GeneratedQuestion entity)
		{
			using (var db = new KTEntities())
			{
				var val = db.GeneratedQuestions.AsEnumerable().DefaultIfEmpty(null).FirstOrDefault(a => a.Id == entity.Id);

				if (val != null)
				{
					val.GeneratedTestId = entity.GeneratedTestId;
					val.QuestionId = entity.QuestionId;
					val.ArgumentText = entity.ArgumentText;
					db.SaveChanges();
				}

				return val;
			}
		}

		public void Delete(Predicate<GeneratedQuestion> predicate)
		{
			using (var db = new KTEntities())
			{
				var questions = db.GeneratedQuestions.AsEnumerable().Where(a => predicate(a));

				foreach (var question in questions)
				{
					db.GeneratedQuestions.DeleteObject(question);
				}
				db.SaveChanges();
			}
		}
	}
}
