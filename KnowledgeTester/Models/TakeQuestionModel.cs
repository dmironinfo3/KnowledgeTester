using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KT.DTOs.Objects;

namespace KnowledgeTester.Models
{
	public class TakeQuestionModel
	{
		public TakeQuestionModel(GeneratedQuestionDto ongoingQuestion)
		{
			Argument = ongoingQuestion.Argument;
			Answers = new List<TakeAnswerModel>();
			Text = ongoingQuestion.Question.Text;
			Id = ongoingQuestion.Question.Id;

			foreach (var q in ongoingQuestion.SelectedAnswers)
			{
				Answers.Add(new TakeAnswerModel
					{
						Id = q.Id,
						IsSelected = q.IsSelected,
						Text = q.Answer.Text
					});
			}
		}

		public TakeQuestionModel()
		{
			
		}

		public List<TakeAnswerModel> Answers { get; set; }
		public string Argument { get; set; }
		public string Text { get; set; }
		public Guid Id { get; set; }
	}
}