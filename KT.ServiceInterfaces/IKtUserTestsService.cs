using System;
using System.Collections.Generic;
using System.ServiceModel;
using KT.DTOs.Objects;

namespace KT.ServiceInterfaces
{
	[ServiceContract]
	public interface IKtUserTestsService
	{
		[OperationContract]
		void DoWork();

		[OperationContract]
		bool IsTestGenerated(Guid id, string username, out GeneratedTestDto test);

		[OperationContract]
		GeneratedTestDto GenerateTest(Guid testId, string username);

		[OperationContract]
		void FinishTest(string username, Guid testId);

		[OperationContract]
		bool IsValidInProgress(Guid testId, string username);

		[OperationContract]
		int GetScore(Guid testId, string username);
	}
}
