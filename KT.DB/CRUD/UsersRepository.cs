using System;
using System.Linq;

namespace KT.DB.CRUD
{
	public class UsersRepository : ICrud<User>
	{
		public User Create(User entity)
		{
			using (var db = new KTEntities())
			{
				db.Users.AddObject(entity);
				db.SaveChanges();
				return entity;
			}
		}

		public User Read(Predicate<User> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var val = db.Users.Include(relatedObjects).DefaultIfEmpty(null).FirstOrDefault(a => predicate(a));
				return val;
			}
		}

		public User[] ReadArray(Predicate<User> predicate, string[] relatedObjects = null)
		{
			using (var db = new KTEntities())
			{
				var val = db.Users.Include(relatedObjects).Where(a => predicate(a));
				return val.ToArray();
			}
		}

		public User Update(User entity)
		{
			using (var db = new KTEntities())
			{
				var val = db.Users.DefaultIfEmpty(null).FirstOrDefault(a => a.Username == entity.Username);

				if (val != null)
				{
					val.Tests = entity.Tests;
					db.SaveChanges();
				}

				return val;
			}
		}

		public void Delete(Predicate<User> predicate)
		{
			throw new Exception("User cannot be deleted!");
		}
	}
}
