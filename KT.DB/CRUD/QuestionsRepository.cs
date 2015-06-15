using System;
using System.Linq;

namespace KT.DB.CRUD
{
	public class QuestionsRepository: ICrud<Question>
	{
		public Question Create(Question entity)
		{
			using (var db = new KTEntities())
			{
				db.Questions.AddObject(entity);
				db.SaveChanges();
				return entity;
			}
		}

		public Question Read(Predicate<Question> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var val = db.Questions.Include(relatedObjects).DefaultIfEmpty(null).FirstOrDefault(a => predicate(a));
				return val;
			}
		}

		public Question[] ReadArray(Predicate<Question> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var val = db.Questions.Include(relatedObjects).Where(a => predicate(a));
				return val.ToArray();
			}
		}

		public Question Update(Question entity)
		{
			using (var db = new KTEntities())
			{
				var val = db.Questions.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == entity.Id);

				if (val != null)
				{
					val.Text = entity.Text;
					val.CorrectArgument = entity.CorrectArgument;
					val.SubcategoryId = entity.SubcategoryId;
					val.MultipleAnswer = entity.MultipleAnswer;
					db.SaveChanges(); 
				}
				return val;
			}
		}

		public void Delete(Predicate<Question> predicate)
		{
			using (var db = new KTEntities())
			{
				var valus = db.Questions.Where(a => predicate(a));

				foreach (var val in valus)
				{
					db.Questions.DeleteObject(val);
					db.SaveChanges();
				}
			}
		}
	}
}
