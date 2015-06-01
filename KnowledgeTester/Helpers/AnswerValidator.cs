using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using KnowledgeTester.Models;

namespace KnowledgeTester.Helpers
{
	public class AnswerValidator : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var items = value as IEnumerable<AnswerModel>;
			if (items == null)
			{
				return true;
			}

			var answerModels = items as IList<AnswerModel> ?? items.ToList();

			var allContainStrings = answerModels.Select(a => a.Text).All(text => !string.IsNullOrEmpty(text));
			var isOneCorrect = answerModels.Any(a => a.IsCorrect);
			var isRightLenth = answerModels.Count >= 3 && answerModels.Count <= 6;

			return allContainStrings && isOneCorrect && isRightLenth;
		}
	}
}