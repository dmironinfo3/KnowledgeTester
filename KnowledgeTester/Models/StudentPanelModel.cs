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

			if (dto.MyTests != null && dto.MyTests.Any())
			{
				foreach (var test in dto.MyTests)
				{
					if (ServicesFactory.GetService<IKtUserTestsService>().IsValidInProgress(test.Id, dto.Username))
					{
						HasOngoingTest = true;
						OngoingTest = new TestModel(test.Test);
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