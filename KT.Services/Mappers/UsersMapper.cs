using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using KT.DB;
using KT.DTOs.Objects;

namespace KT.Services.Mappers
{
	public class UsersMapper: IMapper<UserDto, User>
	{
		public UserDto Map(User entity)
		{
			var instance = new UserDto()
				{
					Username = entity.Username,
					FirstName = entity.FirstName,
					IsAdmin = entity.IsAdmin,
					LastName = entity.LastName,
					Password = entity.Password,
					Email = entity.Password,
					PasswordHint = entity.PasswordHint,
					Categories = (new CategoriesMapper()).Map(entity.Categories),
					MyTests = (new GeneratedTestsMapper()).Map(entity.GeneratedTests),
					Subscriptions = (new TestsMapper()).Map(entity.Tests)
				};

			return instance;
		}

		public User Map(UserDto dto)
		{
			var instance = new User()
			{
				Username = dto.Username,
				FirstName = dto.FirstName,
				IsAdmin = dto.IsAdmin,
				LastName = dto.LastName,
				Password = dto.Password,
				Email = dto.Password,
				PasswordHint = dto.PasswordHint,
				Categories = (new CategoriesMapper()).Map(dto.Categories),
				GeneratedTests = (new GeneratedTestsMapper()).Map(dto.MyTests),
				Tests = (new TestsMapper()).Map(dto.Subscriptions),
			};

			return instance;
		}

		public IEnumerable<UserDto> Map(IEnumerable<User> collection)
		{
			return collection.Select(Map);
		}

		public EntityCollection<User> Map(IEnumerable<UserDto> dtoList)
		{
			var collection = new EntityCollection<User>();
			
			foreach (var userDto in dtoList)
			{
				collection.Add(Map(userDto));
			}

			return collection;
		}
	}
}