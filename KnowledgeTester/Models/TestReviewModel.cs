using System;
using System.Collections.Generic;
using System.Linq;
using KT.DTOs.Objects;

namespace KnowledgeTester.Models
{
	public class TestReviewModel
	{
		public TestReviewModel(TestReviewDto dto)
		{
			Questions = dto.Questions.Select(question => new TestReviewQuestionsModel
			{
				Argument = question.Argument,
				CorrectArgument = question.CorrectArgument,
				Text = question.Text,
				Answers = question.Answers.Select(answer => new TestReviewAnswerModel(answer)).ToArray(),
				Color = question.Answers.Count(a => a.IsCorrect != a.IsSelected) == 0 ? "Green" : "Red",
				
			}).ToArray();

			Score = dto.Score;
		}

		public int Score { get; set; }
		public TestReviewQuestionsModel[] Questions { get; set; }
		public string Username { get; set; }
		public Guid TestId { get; set; }
	}
}