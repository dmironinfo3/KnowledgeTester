using KT.DB;
using KT.DTOs.Objects;
using NUnit.Framework;

namespace KT.Utils.Tests.Mappers
{
	[TestFixture]
	public class GenericMapperTest
	{
		[Test]
		public void Test_Generic_Users_Mapper()
		{
			//var mapper = new GenericMapper<UserDto, User>();

			var user = new UserDto()
				{
					Email = "testEmail",
					FirstName = "firstname",
					LastName = "lastname",
					Username = "username",
					IsAdmin = true,
					Password = "testpass",
					PasswordHint = "testhint"

				};

		//	var userEntity = mapper.Map(user);

			//Assert.IsNotNull(userEntity);

			//Assert.AreEqual(user.Email, userEntity.Email);
			//Assert.AreEqual(user.FirstName, userEntity.FirstName);
			//Assert.AreEqual(user.LastName, userEntity.LastName);
			//Assert.AreEqual(user.Username, userEntity.Username);
			//Assert.AreEqual(user.IsAdmin, userEntity.IsAdmin);
			//Assert.AreEqual(user.Password, userEntity.Password);
			//Assert.AreEqual(user.PasswordHint, userEntity.PasswordHint);
		}
	}
}
