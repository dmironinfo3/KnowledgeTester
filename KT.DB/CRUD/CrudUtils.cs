using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;

namespace KT.DB.CRUD
{
	public static class CrudUtils
	{
		public static IQueryable<T> Include<T>(this IQueryable<T> source, string[] relatedObjects)
		{
			if (relatedObjects == null || !relatedObjects.Any())
			{
				return source;
			}

			var query = (ObjectQuery<T>)source;

			return relatedObjects.Aggregate(query, (current, obj) => current.Include(obj));
		}
	}
}
