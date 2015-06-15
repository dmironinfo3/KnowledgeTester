using System;
using System.Collections.Generic;
using System.ServiceModel;
using KT.DTOs.Objects;

namespace KT.ServiceInterfaces
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IKtQuestionsService" in both code and config file together.
	[ServiceContract]
	public interface IKtQuestionsService
	{
		[OperationContract]
		int GetCountBySubcategory(Guid subCatId);

		[OperationContract]
		void Delete(Guid id);

		[OperationContract]
		QuestionDto[] GetBySubcategory(Guid id);

		[OperationContract]
		double GetUsability(Guid id);

		[OperationContract]
		QuestionDto GetById(Guid id);

		[OperationContract]
		Guid Save(string text, Guid subCatId, Guid? qId = null, bool isMultiple = false, string argument = "");

		[OperationContract]
		QuestionDto[] GetAll();
	}
}
