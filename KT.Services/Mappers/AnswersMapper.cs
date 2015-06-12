using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class AnswersMapper: IMapper<AnswerDto, Answer>
	{
		public AnswerDto Map(Answer entity)
		{
			var instance = new AnswerDto()
			{
				Id = entity.Id,
				Text = entity.Text,
				IsCorrect = entity.IsCorrect
			};

			return instance;
		}

		public Answer Map(AnswerDto dto)
		{
			var instance = new Answer()
			{
				Id = dto.Id,
				Text = dto.Text,
				IsCorrect = dto.IsCorrect
			};

			return instance;
		}

		public IEnumerable<AnswerDto> Map(IEnumerable<Answer> collection)
		{
			return collection.Select(Map);
		}

		public EntityCollection<Answer> Map(IEnumerable<AnswerDto> dtoList)
		{
			var collection = new EntityCollection<Answer>();

			foreach (var dto in dtoList)
			{
				collection.Add(Map(dto));
			}

			return collection;
		}
	}
}