using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class SubcategoriesMapper : IMapper<SubcategoryDto, Subcategory>
	{
		public SubcategoryDto Map(Subcategory entity)
		{
			var instance = new SubcategoryDto()
			{
				Id = entity.Id,
				Questions = (new QuestionsMapper()).Map(entity.Questions),
				Tests = (new TestsMapper()).Map(entity.Tests),
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
				Tests = (new TestsMapper()).Map(dto.Tests),
				CategoryId = dto.CategoryId,
				Name = dto.Name,
			};

			return instance;
		}

		public IEnumerable<SubcategoryDto> Map(IEnumerable<Subcategory> collection)
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
			return new List<SubcategoryDto>();
		}

		public EntityCollection<Subcategory> Map(IEnumerable<SubcategoryDto> dtoList)
		{
			var collection = new EntityCollection<Subcategory>();

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