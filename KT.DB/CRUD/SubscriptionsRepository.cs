using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KT.DB.CRUD
{
	public class SubscriptionsRepository : ISubscriptionsRepository
	{
		public void Subscribe(User u, Test t)
		{
			using (var db = new KTEntities())
			{
				var user = db.Users.First(a => a.Username.Equals(u.Username));
				var test = db.Tests.First(a => a.Id.Equals(t.Id));

				user.Tests.Add(test);
				db.SaveChanges();
			}
		}

		public void Unsubscribe(User u, Test t)
		{
			using (var db = new KTEntities())
			{
				var user = db.Users.First(a => a.Username.Equals(u.Username));
				var test = db.Tests.First(a => a.Id.Equals(t.Id));

				user.Tests.Remove(test);
				db.SaveChanges();
			}
		}
	}

	public interface ISubscriptionsRepository
	{
		void Subscribe(User u, Test t);
		void Unsubscribe(User user, Test test);
	}
}
