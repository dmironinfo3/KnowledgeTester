using System;
using System.Linq;

namespace KT.DB.CRUD
{
	public class AnswersRespository : ICrud<Answer>
	{
		public Answer Create(Answer entity)
		{
			using (var db = new KTEntities())
			{
				db.Answers.AddObject(entity);
				db.SaveChanges();
				return entity;
			}
		}

		public Answer Read(Predicate<Answer> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var val = db.Answers.Include(relatedObjects).AsEnumerable().DefaultIfEmpty(null).FirstOrDefault(a => predicate(a));
				return val;
			}
		}

		public Answer[] ReadArray(Predicate<Answer> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var val = db.Answers.Include(relatedObjects).AsEnumerable().Where(a => predicate(a));
				return val.ToArray();
			}
		}

		public Answer Update(Answer entity)
		{
			using (var db = new KTEntities())
			{
				var val = db.Answers.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == entity.Id);

				if (val != null)
				{
					val.IsCorrect = entity.IsCorrect;
					val.Text = entity.Text;
					val.QuestionId = entity.QuestionId;
					db.SaveChanges();
				}

				return val;
			}
		}

		public void Delete(Predicate<Answer> predicate)
		{
			using (var db = new KTEntities())
			{
				var values = db.Answers.AsEnumerable().Where(a => predicate(a));

				foreach (var val in values)
				{
					db.Answers.DeleteObject(val);
				}
				db.SaveChanges();
			}
		}
	}
}
