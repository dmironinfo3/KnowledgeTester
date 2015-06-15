using System;
using System.Collections.Generic;
using System.ServiceModel;
using KT.DTOs.Objects;

namespace KT.ServiceInterfaces
{
	[ServiceContract]
	public interface IKtTestService
	{
		[OperationContract]
		TestDto[] GetAll();

		[OperationContract]
		void Delete(Guid id);

		[OperationContract]
		TestDto GetById(Guid testId);

		[OperationContract]
		Guid Save(string name, DateTime? startDate, DateTime? endDate, int? duration, Guid subcategoryId, int? questions, Guid? id = null);

		[OperationContract]
		TestDto[] GetAllUpcoming(string username);

		[OperationContract]
		TestDto[] GetFinishedTests(string username);

		[OperationContract]
		string GetSubcategoryName(Guid id);

		[OperationContract]
		TestDto[] GetAllOtherThan(string username);
	}
}
