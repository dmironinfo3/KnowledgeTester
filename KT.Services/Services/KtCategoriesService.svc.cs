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
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KtCategoriesService" in code, svc and config file together.
	public class KtCategoriesService : IKtCategoriesService
	{
		private static readonly ICrud<Category> Repository = CrudFactory<Category>.Get();

		public CategoryDto[] GetAll()
		{
			var relatedObjects = new[] { "User" };

			return (new CategoriesMapper()).Map(Repository.ReadArray(a => true, relatedObjects)).ToArray();
		}

		public CategoryDto GetById(Guid id)
		{
			var relatedObjects = new[] { "User" };

			return (new CategoriesMapper()).Map(Repository.Read(a => a.Id == id, relatedObjects));

		}

		public void Insert(CategoryDto cat)
		{
			var category = (new CategoriesMapper().Map(cat));

			Repository.Create(category);
		}

		public void Delete(Guid id)
		{
			Repository.Delete(a => a.Id == id);
		}

		public Guid Save(string username, string name, Guid? catId = null)
		{
			if (catId.HasValue && !catId.Equals(Guid.Empty))
			{
				var cat = Repository.Read(a => a.Id == catId);

				if (cat != null)
				{
					cat.Name = name;
					cat.CreatedByUser = username;
				}
				cat = Repository.Update(cat);
				return cat.Id;
			}
			else
			{
				var cat = new Category { CreatedByUser = username, Name = name, Id = Guid.NewGuid() };
				cat = Repository.Create(cat);
				return cat.Id;
			}
		}
	}
}
