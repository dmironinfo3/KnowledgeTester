using System;
using System.Collections.Generic;
using System.Linq;

namespace KT.DB.Helpers
{
	public class Subcategories
	{
		public static List<Subcategory> GetAll()
		{
			using (var db = new KTEntities())
			{
				return db.Subcategories.ToList();
			}
		}

		public static void Insert(Subcategory subCat)
		{
			using (var db = new KTEntities())
			{
				db.Subcategories.AddObject(subCat);
				db.SaveChanges();
			}
		}

		public static List<Subcategory> GetByCategory(Guid catId)
		{
			using (var db = new KTEntities())
			{
				var all = db.Subcategories.Where(a=>a.CategoryId == catId);
				return all.ToList();
			}
		}

		public static Subcategory GetById(Guid subCatId)
		{
			using (var db = new KTEntities())
			{
				var subcategory = db.Subcategories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == subCatId);
				return subcategory;
			}
		}

		public static Guid Save(string name, Guid catId, Guid? subcatId = null)
		{
			using (var db = new KTEntities())
			{
				if (subcatId.HasValue && !subcatId.Equals(Guid.Empty))
				{
					var subCat = db.Subcategories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == subcatId);

					if (subCat != null)
						subCat.Name = name;
					db.SaveChanges();
					return subcatId.Value;
				}
				else
				{
					var subCat = new Subcategory { Name = name, Id = Guid.NewGuid(), CategoryId = catId};
					db.Subcategories.AddObject(subCat);
					db.SaveChanges();
					return subCat.Id;
				}
			}
		}

		public static void Delete(Guid id)
		{
			using (var db = new KTEntities())
			{
				var cat = db.Subcategories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				db.Subcategories.DeleteObject(cat);
				db.SaveChanges();
			}
		}

		public static int GetCountByCategory(Guid id)
		{
			using (var db = new KTEntities())
			{
				var count = db.Subcategories.Count(a => a.CategoryId.Equals(id));
				return count;
			}
		}
	}
}
