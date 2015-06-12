using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using KT.DB;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;
using KT.Services.Mappers;

namespace KT.Services.Services
{
	public class KtUsersService : IKtUsersService
	{
		public IEnumerable<UserDto> GetAll()
		{
			using (var db = new KTEntities())
			{
				return (new UsersMapper().Map(db.Users.ToList()));
			}
		}

		public UserDto GetWithTests(string userName)
		{
			using (var db = new KTEntities())
			{
				return (new UsersMapper().Map(db.Users.Include("Tests").
				DefaultIfEmpty(null).
				FirstOrDefault(a => a.Username.Equals(userName))));
			}
		}

		public UserDto GetByKey(string userName)
		{
			
			using (var db = new KTEntities())
			{
				var user = db.Users.
			              DefaultIfEmpty(null).
			              FirstOrDefault(a => a.Username.Equals(userName));

				return user == null ? null : (new UsersMapper()).Map(user);
			}
		}

		public bool IsStudentExistent(string userName)
		{
			var st = GetByKey(userName);
			return st != null;
		}

		public string GetStudentHint(string userName)
		{
			var st = GetByKey(userName);
			if (st != null)
				return st.PasswordHint;
			return "There aren't any hints for " + userName;
		}

		public UserDto Insert(UserDto st)
		{
			using (var db = new KTEntities())
			{
				db.Users.AddObject((new UsersMapper()).Map(st));
				db.SaveChanges();
				return st;
			}
		}

		public bool Authenticate(string username, string password)
		{
			var user = GetByKey(username);

			return user != null && user.Password.Equals(password);
		}

		public void Subscribe(string username, Guid testId)
		{
			using (var db = new KTEntities())
			{
				var user = db.Users.Include("Tests").DefaultIfEmpty(null).FirstOrDefault(a => a.Username.Equals(username));
				var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id.Equals(testId));
				if (user != null)
				{
					user.Tests.Add(test);
					db.SaveChanges();
				}
			}
		}

		public void Unsubscribe(string username, Guid testId)
		{
			using (var db = new KTEntities())
			{
				var user = db.Users.Include("Tests").DefaultIfEmpty(null).FirstOrDefault(a => a.Username.Equals(username));
				var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id.Equals(testId));
				if (user != null)
				{
					user.Tests.Remove(test);
					db.SaveChanges();
				}
			}
		}
	}
}
