using System;
using System.Collections.Generic;
using System.ServiceModel;
using KT.DTOs.Objects;

namespace KT.ServiceInterfaces
{
	[ServiceContract]
	public interface IKtSubcategoriesService
	{
		[OperationContract]
		SubcategoryDto[] GetAll();

		[OperationContract]
		void Insert(SubcategoryDto subCat);

		[OperationContract]
		SubcategoryDto[] GetByCategory(Guid catId);

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
