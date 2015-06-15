using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KT.DTOs.Objects;

namespace KnowledgeTester.Models
{
	public class AnswerModel
	{
		public bool IsCorrect { get; set; }
		public string Text { get; set; }
		public Guid Id { get; set; }

		public AnswerModel(AnswerDto ans)
		{
			IsCorrect = ans.IsCorrect;
			Text = ans.Text;
			Id = ans.Id;
		}

		public AnswerModel()
		{
		}
	}
}