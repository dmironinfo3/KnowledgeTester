using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KnowledgeTester.Ninject;
using KT.ServiceInterfaces;

namespace KnowledgeTester.Code
{
	public static class Utils
	{
		public static string GetFullName(this string username)
		{
			var user = ServicesFactory.GetService<IKtUsersService>().GetByKey(username);

			if (user != null)
			{
				return String.Format("{0} {1} ({2})", user.FirstName, user.LastName, user.Username);
			}

			return String.Empty;
		}
	}
}