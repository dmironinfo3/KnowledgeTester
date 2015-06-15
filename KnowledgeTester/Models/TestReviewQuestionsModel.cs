using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnowledgeTester.Models
{
	public class TestReviewQuestionsModel
	{
		public string Text { get; set; }
		public TestReviewAnswerModel[] Answers { get; set; }
		public string Argument { get; set; }
		public string CorrectArgument { get; set; }
		public string Color { get; set; }
	}
}