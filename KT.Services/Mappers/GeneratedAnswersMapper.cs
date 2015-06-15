using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class GeneratedAnswersMapper : IMapper<GeneratedAnswerDto, GeneratedAnswer>
	{
		public GeneratedAnswerDto Map(GeneratedAnswer entity)
		{
			var instance = new GeneratedAnswerDto
			{
				Id = entity.Id,
				Answer = (new AnswersMapper()).Map(entity.Answer),
				IsSelected = entity.IsSelected,
			};

			return instance;
		}

		public GeneratedAnswer Map(GeneratedAnswerDto dto)
		{
			var instance = new GeneratedAnswer
			{
				Id = dto.Id,
				Answer = (new AnswersMapper()).Map(dto.Answer),
				IsSelected = dto.IsSelected,
			};

			return instance;
		}

		public IEnumerable<GeneratedAnswerDto> Map(IEnumerable<GeneratedAnswer> collection)
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
			return new List<GeneratedAnswerDto>();
		}


		public EntityCollection<GeneratedAnswer> Map(IEnumerable<GeneratedAnswerDto> dtoList)
		{
			var collection = new EntityCollection<GeneratedAnswer>();

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