using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class TestReviewMapper : IMapper<TestReviewDto, GeneratedTest>
	{
		public TestReviewDto Map(GeneratedTest entity)
		{
			var instance = new TestReviewDto()
			{
				Id = entity.Id,
				Username = entity.Username,
				TestId = entity.TestId,
				Questions = (new TestReviewQuestionMapper()).Map(entity.GeneratedQuestions).ToArray(),
				Score = entity.Score
			};

			return instance;
		}

		public GeneratedTest Map(TestReviewDto dto)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TestReviewDto> Map(IEnumerable<GeneratedTest> collection)
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
			return new List<TestReviewDto>();
		}

		public EntityCollection<GeneratedTest> Map(IEnumerable<TestReviewDto> dtoList)
		{
			throw new NotImplementedException();
		}
	}
}