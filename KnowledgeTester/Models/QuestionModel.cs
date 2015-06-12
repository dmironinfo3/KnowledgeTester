using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using KT.DB;
using KT.DB.Objects;
using KT.DTOs.Objects;
using KnowledgeTester.Helpers;

namespace KnowledgeTester.Models
{
	public class QuestionModel
	{
		public QuestionModel(QuestionDto question, string subcategoryName)
		{
			Text = question.Text;
			IsMultiple = question.MultipleResponse;
			Id = question.Id;
			SubcategoryName = subcategoryName;
			CorrectArgument = question.Argument;
			Answers = question.Answers.ToList().Select(ans => new AnswerModel(ans)).ToList();
		}

		public QuestionModel(string subcategoryName)
		{
			Answers = new List<AnswerModel>();
			SubcategoryName = subcategoryName;
		}

		public QuestionModel()
		{
		}

		[Required]
		public string Text { get; set; }

		public bool IsMultiple { get; set; }

		public Guid Id { get; set; }

		public string SubcategoryName { get; set; }

		[Required]
		public string CorrectArgument { get; set; }

		[AnswerValidator(ErrorMessage = "Invalid list of answers")]
		public List<AnswerModel> Answers { get; set; }
	}
}