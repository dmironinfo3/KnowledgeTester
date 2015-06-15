using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;
using KnowledgeTester.Ninject;

namespace KnowledgeTester.Models
{
	public class StudentPanelModel
	{
		public StudentPanelModel(UserDto dto)
		{
			DisplayName = dto.Username;

			if (dto.Subscriptions != null && dto.Subscriptions.Any())
			{
				foreach (var test in dto.Subscriptions)
				{
					if (ServicesFactory.GetService<IKtUserTestsService>().IsValidInProgress(test.Id, dto.Username))
					{
						HasOngoingTest = true;
						OngoingTest = new TestModel(ServicesFactory.GetService<IKtTestService>().GetById(test.Id));
						return;
					}
				}
			}
		}

		public string DisplayName { get; set; }

		public TestModel OngoingTest { get; set; }

		public bool HasOngoingTest { get; set; }
	}
}