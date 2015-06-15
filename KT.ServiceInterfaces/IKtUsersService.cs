using System;
using System.Collections.Generic;
using System.ServiceModel;
using KT.DTOs.Objects;

namespace KT.ServiceInterfaces
{
	[ServiceContract]
	public interface IKtUsersService
	{
		[OperationContract]
		UserDto[] GetAll();

		[OperationContract]
		UserDto GetWithTests(string userName);

		[OperationContract]
		UserDto GetByKey(string userName);

		[OperationContract]
		bool IsStudentExistent(string userName);

		[OperationContract]
		string GetStudentHint(string userName);

		[OperationContract]
		UserDto Insert(UserDto st);

		[OperationContract]
		bool Authenticate(string username, string password);

		[OperationContract]
		void Subscribe(string username, Guid testId);

		[OperationContract]
		void Unsubscribe(string username, Guid testId);

		[OperationContract]
		int GetSubscriptionsFor(Guid id);
	}
}
