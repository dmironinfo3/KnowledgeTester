using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KT.DTOs.Objects;

namespace KnowledgeTester.Models
{
	public class TestReviewAnswerModel
	{
		public TestReviewAnswerModel(TestReviewAnswerDto answer)
		{
			IsSelected = answer.IsSelected;
			IsCorrect = answer.IsCorrect;
			Text = answer.Text;

			Color = IsCorrect ? "Green" : "Red";
		}

		public bool IsSelected { get; set; }
		public string Text { get; set; }
		public bool IsCorrect { get; set; }
		public string Color { get; set; }
	}
}