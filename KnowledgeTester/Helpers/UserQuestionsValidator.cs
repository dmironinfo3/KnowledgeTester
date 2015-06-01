using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using KnowledgeTester.Models;

namespace KnowledgeTester.Helpers
{
	public class UserQuestionsValidator : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var items = value as IEnumerable<TakeQuestionModel>;
			if (items == null)
			{
				return true;
			}

			var answerModels = items as IList<TakeQuestionModel> ?? items.ToList();

			foreach (var takeQuestionModel in answerModels)
			{
				var argumentIsPresent = !String.IsNullOrEmpty(takeQuestionModel.Argument);
				var isOneSelected = takeQuestionModel.Answers.Any(a => a.IsSelected);
				if (!argumentIsPresent || !isOneSelected)
				{
					return false;
				}
			}
			return true;
		}
	}
}