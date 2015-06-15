using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;

namespace KT.DB.CRUD
{
	public interface ICrud<T> where T : EntityObject
	{
		T Create(T entity);

		T Read(Predicate<T> predicate, string[] relatedObjects = null);

		T[] ReadArray(Predicate<T> predicate, string[] relatedObjects = null);

		T Update(T entity);

		void Delete(Predicate<T> predicate);
	}
}
