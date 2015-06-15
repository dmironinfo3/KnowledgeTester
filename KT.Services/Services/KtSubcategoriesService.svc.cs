using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using KT.DB;
using KT.DB.CRUD;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;
using KT.Services.Mappers;

namespace KT.Services.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KtSubcategoriesService" in code, svc and config file together.
	public class KtSubcategoriesService : IKtSubcategoriesService
	{
		private static readonly ICrud<Subcategory> Repository = CrudFactory<Subcategory>.Get();

		public SubcategoryDto[] GetAll()
		{
			var relatedObjects = new[] { "Questions" };
			return (new SubcategoriesMapper()).Map(Repository.ReadArray(a => true, relatedObjects)).ToArray();
		}

		public void Insert(SubcategoryDto subCat)
		{
			var entity = (new SubcategoriesMapper()).Map(subCat);
			Repository.Create(entity);
		}

		public SubcategoryDto[] GetByCategory(Guid catId)
		{
			var relatedObjects = new[] { "Tests", "Questions" };

			var all = Repository.ReadArray(a => a.CategoryId == catId, relatedObjects);
			var mapped = (new SubcategoriesMapper()).Map(all);

			return mapped.ToArray();
		}

		public SubcategoryDto GetById(Guid subCatId)
		{
			var relatedObjects = new[] { "Questions" };
			var subcategory = Repository.Read(a => a.Id == subCatId, relatedObjects);
			return (new SubcategoriesMapper()).Map(subcategory);
		}

		public Guid Save(string name, Guid catId, Guid? subcatId = null)
		{
			if (subcatId.HasValue && !subcatId.Equals(Guid.Empty))
			{
				var subCat = Repository.Read(a => a.Id == subcatId);

				if (subCat != null)
					subCat.Name = name;

				Repository.Update(subCat);

				return subcatId.Value;
			}
			else
			{
				var subCat = new Subcategory { Name = name, Id = Guid.NewGuid(), CategoryId = catId };
				Repository.Create(subCat);
				return subCat.Id;
			}
		}


		public void Delete(Guid id)
		{
			Repository.Delete(a => a.Id == id);
		}

		public int GetCountByCategory(Guid id)
		{
			var count = Repository.ReadArray(a => a.CategoryId.Equals(id)).Length;
			return count;
		}
	}
}
