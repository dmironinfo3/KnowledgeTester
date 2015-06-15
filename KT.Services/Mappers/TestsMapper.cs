using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class TestsMapper : IMapper<TestDto, Test>
	{
		public TestDto Map(Test entity)
		{
			var instance = new TestDto()
			{
				Id = entity.Id,
				Name = entity.Name,
				StartTime = entity.StartDate,
				EndTime = entity.EndDate,
				SubcategoryId = entity.SubcategoryId,
				Duration = entity.MinutesDuration,
				Questions = entity.QuestionCount.Value,
			};

			return instance;
		}

		public Test Map(TestDto dto)
		{
			var instance = new Test()
			{
				Id = dto.Id,
				Name = dto.Name,
				StartDate = dto.StartTime,
				EndDate = dto.EndTime,
				SubcategoryId = dto.SubcategoryId,
				MinutesDuration = dto.Duration,
				QuestionCount = dto.Questions
			};

			return instance;
		}

		public IEnumerable<TestDto> Map(IEnumerable<Test> collection)
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
			return new List<TestDto>();
		}

		public EntityCollection<Test> Map(IEnumerable<TestDto> dtoList)
		{
			var collection = new EntityCollection<Test>();

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

