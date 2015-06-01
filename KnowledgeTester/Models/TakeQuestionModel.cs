using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KT.DB.Objects;

namespace KnowledgeTester.Models
{
	public class TakeQuestionModel
	{
		public TakeQuestionModel(OngoingQuestion ongoingQuestion)
		{
			Argument = ongoingQuestion.Argument;
			Answers = new List<TakeAnswerModel>();
			Text = ongoingQuestion.Text;
			Id = ongoingQuestion.Id;

			foreach (var q in ongoingQuestion.Answers)
			{
				Answers.Add(new TakeAnswerModel()
					{
						Id = q.Id,
						IsSelected = q.IsSelected,
						Text = q.Text
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