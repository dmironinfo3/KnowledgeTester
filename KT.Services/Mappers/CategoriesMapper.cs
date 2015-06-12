using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class CategoriesMapper: IMapper<CategoryDto, Category>
	{
		public CategoryDto Map(Category entity)
		{
			var instance = new CategoryDto()
			{
				Id = entity.Id,
				Name = entity.Name,
				CreatedBy = (new UsersMapper()).Map(entity.User)
			};

			return instance;
		}

		public Category Map(CategoryDto dto)
		{
			var instance = new Category()
			{
				Id = dto.Id,
				Name = dto.Name,
				User = (new UsersMapper()).Map(dto.CreatedBy)
			};

			return instance;
		}

		public IEnumerable<CategoryDto> Map(IEnumerable<Category> collection)
		{
			return collection.Select(Map);
		}

		public EntityCollection<Category> Map(IEnumerable<CategoryDto> dtoList)
		{
			var collection = new EntityCollection<Category>();

			foreach (var cat in dtoList)
			{
				collection.Add(Map(cat));
			}

			return collection;
		}
	}
}