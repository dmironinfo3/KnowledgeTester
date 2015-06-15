using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class CategoriesMapper : IMapper<CategoryDto, Category>
	{
		public CategoryDto Map(Category entity)
		{
			var instance = new CategoryDto()
			{
				Id = entity.Id,
				Name = entity.Name,
				CreatedBy = entity.CreatedByUser
			};

			return instance;
		}

		public Category Map(CategoryDto dto)
		{
			var instance = new Category()
			{
				Id = dto.Id,
				Name = dto.Name,
				CreatedByUser = dto.CreatedBy
			};

			return instance;
		}

		public IEnumerable<CategoryDto> Map(IEnumerable<Category> collection)
		{
			try
			{
				if (collection != null && collection.Any())
				{
					return collection.Select(Map);
				}
			}
			catch
			{
				// ignored
			}
			return new List<CategoryDto>();
		}

		public EntityCollection<Category> Map(IEnumerable<CategoryDto> dtoList)
		{
			var collection = new EntityCollection<Category>();

			if (dtoList != null && dtoList.Any())
			{
				foreach (var cat in dtoList)
				{
					collection.Add(Map(cat));
				}
			}

			return collection;
		}
	}
}