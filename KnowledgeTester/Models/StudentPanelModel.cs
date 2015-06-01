using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnowledgeTester.Models
{
	public class StudentPanelModel
	{
		public StudentPanelModel(KT.DB.Student st)
		{
			DisplayName = st.Username;

			if (st.Tests != null && st.Tests.Count > 0)
			{
				foreach (var test in st.Tests)
				{
					if (KT.DB.Helpers.StudentTests.IsValidInProgress(test.Id,st.Username))
					{
						
							HasOngoingTest = true;
							OngoingTest = new TestModel(test);
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