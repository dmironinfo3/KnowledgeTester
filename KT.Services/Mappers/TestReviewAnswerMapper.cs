using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class TestReviewAnswerMapper:IMapper<TestReviewAnswerDto, GeneratedAnswer>
	{
		public TestReviewAnswerDto Map(GeneratedAnswer entity)
		{
			var instance = new TestReviewAnswerDto()
			{
				Text = entity.Answer.Text,
				IsCorrect = entity.Answer.IsCorrect,
				IsSelected = entity.IsSelected
			};

			return instance;
		}

		public GeneratedAnswer Map(TestReviewAnswerDto dto)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TestReviewAnswerDto> Map(IEnumerable<GeneratedAnswer> collection)
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
			return new List<TestReviewAnswerDto>();
		}

		public EntityCollection<GeneratedAnswer> Map(IEnumerable<TestReviewAnswerDto> dtoList)
		{
			throw new NotImplementedException();
		}
	}
}