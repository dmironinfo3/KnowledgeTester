using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using KT.DB.Objects;
using KT.DTOs.Objects;
using KnowledgeTester.Helpers;

namespace KnowledgeTester.Models
{
	public class TakeTestModel
	{
		public TakeTestModel(GeneratedTestDto test)
		{
			Username = test.User.Username;
			TestId = test.Test.Id;
			Questions = new List<TakeQuestionModel>();

			DurationMinutes = test.Test.Duration;

			foreach (var q in test.GeneratedQuestions)
			{
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