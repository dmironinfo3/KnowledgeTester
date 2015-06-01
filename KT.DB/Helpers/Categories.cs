using System;
using System.Collections.Generic;
using System.Linq;

namespace KT.DB.Helpers
{
	public class Categories
	{
		public static List<Category> GetAll()
		{
			using (var db = new KTEntities())
			{
				return db.Categories.ToList();
			}
		}

		public static Category GetById(Guid id)
		{
			using (var db = new KTEntities())
			{
				return db.Categories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);
			}
		}

		public static void Insert(Category cat)
		{
			using (var db = new KTEntities())
			{
				db.Categories.AddObject(cat);
				db.SaveChanges();
			}
		}

		public static void Delete(Guid id)
		{
			using (var db = new KTEntities())
			{
				var cat = db.Categories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				db.Categories.DeleteObject(cat);
				db.SaveChanges();
			}
		}

		public static Guid Save(string name, Guid? catId = null)
		{
			using (var db = new KTEntities())
			{
				if (catId.HasValue && !catId.Equals(Guid.Empty))
				{
					var cat = db.Categories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == catId);

					if (cat != null)
						cat.Name = name;
					db.SaveChanges();
					return catId.Value;
				}
				else
				{
					var cat = new Category() {Name = name, Id = Guid.NewGuid()};
					db.Categories.AddObject(cat);
					db.SaveChanges();
					return cat.Id;
				}
			}
		}
	}
}
