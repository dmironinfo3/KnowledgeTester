using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;

namespace KT.DB.CRUD
{
	public class CrudFactory<T> where T : EntityObject
	{
		public static ICrud<T> Get()
		{
			var repository = typeof(ICrud<T>)
				.Assembly.GetTypes()
				.FirstOrDefault(t => !t.IsAbstract);

			if (repository != null)
				return (ICrud<T>)Activator.CreateInstance(repository);

			throw new Exception("Not implemented type!");
		}
	}
}
