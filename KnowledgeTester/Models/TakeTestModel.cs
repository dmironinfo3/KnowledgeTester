using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using KT.DB.Objects;
using KnowledgeTester.Helpers;

namespace KnowledgeTester.Models
{
	public class TakeTestModel
	{
		public TakeTestModel(OngoingTest test)
		{
			Username = test.Username;
			TestId = test.TestId;
			Questions = new List<TakeQuestionModel>();

			DurationMinutes = test.Duration;

			foreach (var q in test.Questions)
			{
				var t = test.Questions.First(a => a.Id == q.Id);

				Questions.Add(new TakeQuestionModel(q));
			}
		}

		public TakeTestModel()
		{

		}

		public string Username { get; set; }
		public Guid TestId { get; set; }

		[UserQuestionsValidator]
		public List<TakeQuestionModel> Questions { get; set; }

		public int DurationMinutes { get; set; }

		public bool TimeElapsed { get; set; }

		public string ToJson()
		{
			return new JavaScriptSerializer().Serialize(this);
		}
	}
}