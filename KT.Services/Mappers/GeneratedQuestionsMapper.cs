using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class GeneratedQuestionsMapper: IMapper<GeneratedQuestionDto, GeneratedQuestion>
	{
		public GeneratedQuestionDto Map(GeneratedQuestion entity)
		{
			var instance = new GeneratedQuestionDto()
			{
				Question = (new QuestionsMapper()).Map(entity.Question),
				SelectedAnswers = (new GeneratedAnswersMapper()).Map(entity.GeneratedAnswers)
			};

			return instance;
		}

		public GeneratedQuestion Map(GeneratedQuestionDto dto)
		{
			var instance = new GeneratedQuestion()
			{
				Question = (new QuestionsMapper()).Map(dto.Question),
				GeneratedAnswers = (new GeneratedAnswersMapper()).Map(dto.SelectedAnswers)
			};

			return instance;
		}

		public IEnumerable<GeneratedQuestionDto> Map(IEnumerable<GeneratedQuestion> collection)
		{
			return collection.Select(Map);
		}

		public EntityCollection<GeneratedQuestion> Map(IEnumerable<GeneratedQuestionDto> dtoList)
		{
			var collection = new EntityCollection<GeneratedQuestion>();

			foreach (var dto in dtoList)
			{
				collection.Add(Map(dto));
			}

			return collection;
		}
	}
}