using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class GeneratedTestsMapper : IMapper<GeneratedTestDto, GeneratedTest>
	{
		public GeneratedTestDto Map(GeneratedTest entity)
		{
			var instance = new GeneratedTestDto()
			{
				Id = entity.Id,
				IsFinished = entity.IsFinished,
				IsValidated = entity.IsVerified,
				Username = entity.Username,
				MaxScore = entity.MaxScore,
				Score = entity.Score,
				TestId = entity.TestId,
				GeneratedQuestions = (new GeneratedQuestionsMapper()).Map(entity.GeneratedQuestions)
			};

			return instance;
		}

		public GeneratedTest Map(GeneratedTestDto dto)
		{
			var instance = new GeneratedTest()
			{
				Id = dto.Id,
				IsFinished = dto.IsFinished,
				IsVerified = dto.IsValidated,
				Username = dto.Username,
				MaxScore = dto.MaxScore,
				Score = dto.Score,
				TestId = dto.TestId,
				GeneratedQuestions = (new GeneratedQuestionsMapper()).Map(dto.GeneratedQuestions)
			};

			return instance;
		}

		public IEnumerable<GeneratedTestDto> Map(IEnumerable<GeneratedTest> collection)
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
			return new List<GeneratedTestDto>();
		}

		public EntityCollection<GeneratedTest> Map(IEnumerable<GeneratedTestDto> dtoList)
		{
			var collection = new EntityCollection<GeneratedTest>();

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
