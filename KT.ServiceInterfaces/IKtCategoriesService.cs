using System;
using System.Collections.Generic;
using System.ServiceModel;
using KT.DTOs.Objects;

namespace KT.ServiceInterfaces
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IKtCategoriesService" in both code and config file together.
	[ServiceContract]
	public interface IKtCategoriesService
	{
		[OperationContract]
		IEnumerable<CategoryDto> GetAll();

		[OperationContract]
		CategoryDto GetById(Guid id);

		[OperationContract]
		void Insert(CategoryDto cat);

		[OperationContract]
		void Delete(Guid id);

		[OperationContract]
		Guid Save(string name, Guid? catId = null);
	}
}
