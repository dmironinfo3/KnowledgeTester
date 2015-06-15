using System;
using System.Data.Objects;
using System.Linq;

namespace KT.DB.CRUD
{
	public class CategoriesRepository:ICrud<Category>
	{
		public Category Create(Category entity)
		{
			using (var db = new KTEntities())
			{
				db.Categories.AddObject(entity);
				db.SaveChanges();
				return entity;
			}
		}

		public Category Read(Predicate<Category> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var val = db.Categories.Include(relatedObjects).DefaultIfEmpty(null).FirstOrDefault(a => predicate(a));
				return val;
			}
		}

		public Category[] ReadArray(Predicate<Category> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var val = db.Categories.Include(relatedObjects).Where(a => predicate(a));
				return val.ToArray();
			}
		}

		public Category Update(Category entity)
		{
			using (var db = new KTEntities())
			{
				var val = db.Categories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == entity.Id);

				if (val != null)
				{
					val.Name = entity.Name;
					val.CreatedByUser = entity.CreatedByUser;
					db.SaveChanges();
				}

				return val;
			}
		}

		public void Delete(Predicate<Category> predicate)
		{
			using (var db = new KTEntities())
			{
				var values = db.Categories.Where(a => predicate(a));

				foreach (var val in values)
				{
					db.Categories.DeleteObject(val);
					db.SaveChanges();
				}
			}
		}
	}
}
