using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class SubcategoriesMapper: IMapper<SubcategoryDto, Subcategory>
	{
		public SubcategoryDto Map(Subcategory entity)
		{
			var instance = new SubcategoryDto()
			{
				Id = entity.Id,
				Questions = (new QuestionsMapper()).Map(entity.Questions),
				CategoryId = entity.CategoryId,
				Name = entity.Name,
			};

			return instance;
		}

		public Subcategory Map(SubcategoryDto dto)
		{
			var instance = new Subcategory()
			{
				Id = dto.Id,
				Questions = (new QuestionsMapper()).Map(dto.Questions),
				CategoryId = dto.CategoryId,
				Name = dto.Name,
			};

			return instance;
		}

		public IEnumerable<SubcategoryDto> Map(IEnumerable<Subcategory> collection)
		{
			return collection.Select(Map);
		}

		public EntityCollection<Subcategory> Map(IEnumerable<SubcategoryDto> dtoList)
		{
			var collection = new EntityCollection<Subcategory>();

			foreach (var dto in dtoList)
			{
				collection.Add(Map(dto));
			}

			return collection;
		}
	}
}