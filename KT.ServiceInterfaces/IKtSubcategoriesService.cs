using System;
using System.Collections.Generic;
using System.ServiceModel;
using KT.DTOs.Objects;

namespace KT.ServiceInterfaces
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IKtSubcategoriesService" in both code and config file together.
	[ServiceContract]
	public interface IKtSubcategoriesService
	{
		[OperationContract]
		IEnumerable<SubcategoryDto> GetAll();

		[OperationContract]
		void Insert(SubcategoryDto subCat);

		[OperationContract]
		IEnumerable<SubcategoryDto> GetByCategory(Guid catId);

		[OperationContract]
		SubcategoryDto GetById(Guid subCatId);

		[OperationContract]
		Guid Save(string name, Guid catId, Guid? subcatId = null);

		[OperationContract]
		void Delete(Guid id);

		[OperationContract]
		int GetCountByCategory(Guid id);
	}
}
