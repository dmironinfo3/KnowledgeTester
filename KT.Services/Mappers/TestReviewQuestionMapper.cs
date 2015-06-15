using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class TestReviewQuestionMapper:IMapper<TestReviewQuestionDto,GeneratedQuestion>
	{
		public TestReviewQuestionDto Map(GeneratedQuestion entity)
		{
			var instance = new TestReviewQuestionDto()
			{
				Id = entity.Id,
				Argument = entity.ArgumentText,
				CorrectArgument = entity.Question.CorrectArgument,
				Text = entity.Question.Text,
				Answers = (new TestReviewAnswerMapper()).Map(entity.GeneratedAnswers).ToArray()
			};
			return instance;
		}

		public GeneratedQuestion Map(TestReviewQuestionDto dto)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TestReviewQuestionDto> Map(IEnumerable<GeneratedQuestion> collection)
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
			return new List<TestReviewQuestionDto>();
		}

		public EntityCollection<GeneratedQuestion> Map(IEnumerable<TestReviewQuestionDto> dtoList)
		{
			throw new NotImplementedException();
		}
	}
}