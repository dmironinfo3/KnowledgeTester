using System;
using System.ServiceModel;

namespace KT.ServiceInterfaces
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface IKtAnswersService
	{
		[OperationContract]
		void AddEmpyFor(Guid id);

		[OperationContract]
		void Delete(Guid id);

		[OperationContract]
		Guid Save(Guid id, Guid questionId, string text, bool isCorrect);

		[OperationContract]
		void SaveTestAnswer(Guid ansId, bool selected);
	}
}
