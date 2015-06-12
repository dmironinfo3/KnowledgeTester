using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using KT.DB;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;
using KT.Services.Mappers;

namespace KT.Services.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KtSubcategoriesService" in code, svc and config file together.
	public class KtSubcategoriesService : IKtSubcategoriesService
	{
		public IEnumerable<SubcategoryDto> GetAll()
		{
			using (var db = new KTEntities())
			{
				return (new SubcategoriesMapper()).Map(db.Subcategories.ToList());
			}
		}

		public void Insert(SubcategoryDto subCat)
		{
			using (var db = new KTEntities())
			{
				db.Subcategories.AddObject((new SubcategoriesMapper()).Map(subCat));
				db.SaveChanges();
			}
		}

		public IEnumerable<SubcategoryDto> GetByCategory(Guid catId)
		{
			using (var db = new KTEntities())
			{
				var all = db.Subcategories.Where(a => a.CategoryId == catId);
				return (new SubcategoriesMapper()).Map(all);
			}
		}

		public SubcategoryDto GetById(Guid subCatId)
		{
			using (var db = new KTEntities())
			{
				var subcategory = db.Subcategories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == subCatId);
				return (new SubcategoriesMapper()).Map(subcategory);
			}
		}

		public Guid Save(string name, Guid catId, Guid? subcatId = null)
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
					var subCat = new Subcategory { Name = name, Id = Guid.NewGuid(), CategoryId = catId };
					db.Subcategories.AddObject(subCat);
					db.SaveChanges();
					return subCat.Id;
				}
			}
		}

		public void Delete(Guid id)
		{
			using (var db = new KTEntities())
			{
				var cat = db.Subcategories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				db.Subcategories.DeleteObject(cat);
				db.SaveChanges();
			}
		}

		public int GetCountByCategory(Guid id)
		{
			using (var db = new KTEntities())
			{
				var count = db.Subcategories.Count(a => a.CategoryId.Equals(id));
				return count;
			}
		}
	}
}
