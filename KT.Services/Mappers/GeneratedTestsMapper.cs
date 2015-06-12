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
				User = (new UsersMapper()).Map(entity.User),
				MaxScore = entity.MaxScore,
				Score = entity.Score,
				Test = (new TestsMapper()).Map(entity.Test),
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
				User = (new UsersMapper()).Map(dto.User),
				MaxScore = dto.MaxScore,
				Score = dto.Score,
				Test = (new TestsMapper()).Map(dto.Test),
				GeneratedQuestions = (new GeneratedQuestionsMapper()).Map(dto.GeneratedQuestions)
			};

			return instance;
		}

		public IEnumerable<GeneratedTestDto> Map(IEnumerable<GeneratedTest> collection)
		{
			return collection.Select(Map);
		}

		public EntityCollection<GeneratedTest> Map(IEnumerable<GeneratedTestDto> dtoList)
		{
			var collection = new EntityCollection<GeneratedTest>();

			foreach (var dto in dtoList)
			{
				collection.Add(Map(dto));
			}

			return collection;
		}
	}
}
