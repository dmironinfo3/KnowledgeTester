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
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KtCategoriesService" in code, svc and config file together.
	public class KtCategoriesService : IKtCategoriesService
	{
		public IEnumerable<CategoryDto> GetAll()
		{
			using (var db = new KTEntities())
			{
				return (new CategoriesMapper()).Map(db.Categories.ToList());
			}
		}

		public CategoryDto GetById(Guid id)
		{
			using (var db = new KTEntities())
			{
				return (new CategoriesMapper()).Map(db.Categories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id));
			}
		}

		public void Insert(CategoryDto cat)
		{
			using (var db = new KTEntities())
			{
				db.Categories.AddObject((new CategoriesMapper().Map(cat)));
				db.SaveChanges();
			}
		}

		public void Delete(Guid id)
		{
			using (var db = new KTEntities())
			{
				var cat = db.Categories.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				db.Categories.DeleteObject(cat);
				db.SaveChanges();
			}
		}

		public Guid Save(string name, Guid? catId = null)
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
					var cat = new Category { Name = name, Id = Guid.NewGuid() };
					db.Categories.AddObject(cat);
					db.SaveChanges();
					return cat.Id;
				}
			}
		}
	}
}
