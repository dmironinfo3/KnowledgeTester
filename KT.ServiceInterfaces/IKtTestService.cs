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
		IEnumerable<TestDto> GetAll();

		[OperationContract]
		void Delete(Guid id);

		[OperationContract]
		TestDto GetById(Guid testId);

		[OperationContract]
		Guid Save(string name, DateTime? startDate, DateTime? endDate, int? duration, Guid subcategoryId, Guid? id = null);

		[OperationContract]
		IEnumerable<TestDto> GetAllUpcoming(string username);

		[OperationContract]
		IEnumerable<TestDto> GetFinishedTests(string username);
	}
}
