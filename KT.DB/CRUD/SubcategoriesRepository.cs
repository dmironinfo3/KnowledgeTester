using System;
using System.Linq;

namespace KT.DB.CRUD
{
	public class SubcategoriesRepository : ICrud<Subcategory>
	{
		public Subcategory Create(Subcategory entity)
		{
			using (var db = new KTEntities())
			{
				db.Subcategories.AddObject(entity);
				db.SaveChanges();
				return entity;
			}
		}

		public Subcategory Read(Predicate<Subcategory> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var test = db.Subcategories.Include(relatedObjects).DefaultIfEmpty(null).FirstOrDefault(a => predicate(a));
				return test;
			}
		}

		public Subcategory[] ReadArray(Predicate<Subcategory> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var test = db.Subcategories.Include(relatedObjects).Where(a => predicate(a));
				return test.ToArray();
			}
		}

		public Subcategory Update(Subcategory entity)
		{
			using (var db = new KTEntities())
			{
				var val = db.Subcategories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == entity.Id);

				if (val != null)
				{
					val.Name = entity.Name;
					val.CategoryId = entity.CategoryId;
					db.SaveChanges();
				}
				return val;
			}
		}

		public void Delete(Predicate<Subcategory> predicate)
		{
			using (var db = new KTEntities())
			{
				var subcategories = db.Subcategories.Where(a => predicate(a));

				foreach (var subcat in subcategories)
				{
					db.Subcategories.DeleteObject(subcat);
					db.SaveChanges();
				}
			}
		}
	}
}
