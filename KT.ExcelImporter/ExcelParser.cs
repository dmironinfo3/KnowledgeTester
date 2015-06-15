using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KT.DTOs.Objects;
using OfficeOpenXml;

namespace KT.ExcelImporter
{
	public class ExcelParser : IExcelParser
	{
		public IEnumerable<QuestionDto> Parse(StreamReader str)
		{
			var dtos = new List<QuestionDto>();
			using (var package = new ExcelPackage())
			{
				package.Load(str.BaseStream);

				var worksheet = package.Workbook.Worksheets.First();

				var start = worksheet.Dimension.Start.Row;
				var end = worksheet.Dimension.End.Row;

				for (int i = start; i <= end; i++)
				{
					var c2 = worksheet.Cells[i, 3].Value;

					if (!(c2 is string)) continue;

					if (!c2.ToString().Equals("Is Correct Answer")) continue;

					var question = GetQuestion(worksheet, i);

					i = GetAnswers(i, worksheet, question);

					i = GetArgument(i, worksheet, question);

					question.MultipleResponse = question.Answers.Count(a => a.IsCorrect) > 1;

					dtos.Add(question);
				}
				return dtos;
			}
		}

		private int GetArgument(int i, ExcelWorksheet worksheet, QuestionDto question)
		{
			var c1 = worksheet.Cells[i, 2].Value;

			if (c1 is string)
			{
				question.Argument = c1.ToString();
			}

			return i + 1;
		}

		private static int GetAnswers(int i, ExcelWorksheet worksheet, QuestionDto question)
		{
			var answers = new List<AnswerDto>();

			var isAnswer = true;

			while (isAnswer)
			{
				i = i + 1;
				var c1 = worksheet.Cells[i, 2].Value;
				var c2 = worksheet.Cells[i, 3].Value;

				if (!(c1 is string) || !(c2 is bool))
				{
					isAnswer = false;
					continue;
				}

				var ansText = c1.ToString();
				var isCorrect = Convert.ToBoolean(c2);

				var answer = new AnswerDto()
				{
					Id = Guid.NewGuid(),
					QuestionId = question.Id,
					Text = ansText,
					IsCorrect = Convert.ToBoolean(isCorrect)
				};
				answers.Add(answer);
			}

			question.Answers = answers;
			return i;
		}

		private static QuestionDto GetQuestion(ExcelWorksheet worksheet, int i)
		{
			var question = new QuestionDto
			{
				Id = Guid.NewGuid(),
				Text = worksheet.Cells[i, 2].Value.ToString(),
			};
			return question;
		}
	}
}
