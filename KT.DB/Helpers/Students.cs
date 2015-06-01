using System;
using System.Collections.Generic;
using System.Linq;

namespace KT.DB.Helpers
{
	public class Students
	{
		public static List<Student> GetAll()
		{
			using (var db = new KTEntities())
			{
				return db.Students.ToList();
			}
		}

		public static Student GetWithTests(string userName)
		{
			using (var db = new KTEntities())
			{
				return db.Students.Include("Tests").DefaultIfEmpty(null).FirstOrDefault(a => a.Username.Equals(userName));
			}
		}

		public static Student GetByKey(string userName)
		{
			using (var db = new KTEntities())
			{
				return db.Students.DefaultIfEmpty(null).FirstOrDefault(a => a.Username.Equals(userName));
			}
		}

		public static bool IsStudentExistent(string userName)
		{
			var st = GetByKey(userName);
			return st != null;
		}

		public static string GetStudentHint(string userName)
		{
			var st = GetByKey(userName);
			if (st != null)
				return st.PasswordHint;
			return "There aren't any hints for " + userName;
		}

		public static Student Insert(Student st)
		{
			using (var db = new KTEntities())
			{
				db.Students.AddObject(st);
				db.SaveChanges();
				return st;
			}
		}

		public static bool Authenticate(string username, string password)
		{
			var user = GetByKey(username);

			return user != null && user.Password.Equals(password);
		}

		public static void Subscribe(string username, Guid testId)
		{
			using (var db = new KTEntities())
			{
				var user = db.Students.Include("Tests").DefaultIfEmpty(null).FirstOrDefault(a => a.Username.Equals(username));
				var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id.Equals(testId));
				if (user != null)
				{
					user.Tests.Add(test);
					db.SaveChanges();
				}
			}
		}

		public static void Unsubscribe(string username, Guid testId)
		{
			using (var db = new KTEntities())
			{
				var user = db.Students.Include("Tests").DefaultIfEmpty(null).FirstOrDefault(a => a.Username.Equals(username));
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
