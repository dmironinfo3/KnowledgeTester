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
				Subcategory = (new SubcategoriesMapper()).Map(entity.Subcategory),
				SubscribedUsers = (new UsersMapper()).Map(entity.Users),
				Duration = entity.MinutesDuration
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
				Subcategory = (new SubcategoriesMapper()).Map(dto.Subcategory),
				Users = (new UsersMapper()).Map(dto.SubscribedUsers),
				MinutesDuration = dto.Duration
			};

			return instance;
		}

		public IEnumerable<TestDto> Map(IEnumerable<Test> collection)
		{
			return collection.Select(Map);
		}

		public EntityCollection<Test> Map(IEnumerable<TestDto> dtoList)
		{
			var collection = new EntityCollection<Test>();

			foreach (var dto in dtoList)
			{
				collection.Add(Map(dto));
			}

			return collection;
		}
	}
}
