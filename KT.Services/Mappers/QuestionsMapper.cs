using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class QuestionsMapper: IMapper<QuestionDto, Question>
	{
		public QuestionDto Map(Question entity)
		{
			var instance = new QuestionDto()
			{
				Id = entity.Id,
				Answers = (new AnswersMapper()).Map(entity.Answers),
				Text = entity.Text,
				Argument = entity.CorrectArgument,
				MultipleResponse = entity.MultipleAnswer,
				SubcategoryId =entity.SubcategoryId
			};

			return instance;
		}

		public Question Map(QuestionDto dto)
		{
			var instance = new Question()
			{
				Id = dto.Id,
				Answers = (new AnswersMapper()).Map(dto.Answers),
				Text = dto.Text,
				CorrectArgument = dto.Argument,
				MultipleAnswer = dto.MultipleResponse,
				SubcategoryId = dto.SubcategoryId
			};

			return instance;
		}

		public IEnumerable<QuestionDto> Map(IEnumerable<Question> collection)
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
			return new List<QuestionDto>();
		}

		public EntityCollection<Question> Map(IEnumerable<QuestionDto> dtoList)
		{
			var collection = new EntityCollection<Question>();

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