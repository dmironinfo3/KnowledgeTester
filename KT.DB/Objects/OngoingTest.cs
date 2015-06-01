using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KT.DB.Objects
{
	public class OngoingTest
	{
		public OngoingTest(StudentTest studentTests)
		{
			Username = studentTests.StudentUsername;
			TestId = studentTests.TestId;

			Questions = new List<OngoingQuestion>();

			Duration = studentTests.Test.MinutesDuration;

			BindQuestions(studentTests);

		}

		private void BindQuestions(StudentTest studentTests)
		{
			Questions.Add(item: new OngoingQuestion()
			{
				Id = studentTests.Q1Id,
				Argument = GetSelectedArgument(studentTests.Q1Answer),
				Answers = Answers(studentTests.Q1Answer, studentTests.Question),
				Text = studentTests.Question.Text
			}
			);
			Questions.Add(new OngoingQuestion()
			{
				Id = studentTests.Q2Id,
				Argument = GetSelectedArgument(studentTests.Q2Answer),
				Answers = Answers(studentTests.Q2Answer, studentTests.Question1),
				Text = studentTests.Question1.Text
			}
			);
			Questions.Add(new OngoingQuestion()
			{
				Id = studentTests.Q3Id,
				Argument = GetSelectedArgument(studentTests.Q3Answer),
				Answers = Answers(studentTests.Q3Answer, studentTests.Question2),
				Text = studentTests.Question2.Text
			}
			);
			Questions.Add(new OngoingQuestion()
			{
				Id = studentTests.Q4Id,
				Argument = GetSelectedArgument(studentTests.Q4Answer),
				Answers = Answers(studentTests.Q4Answer, studentTests.Question3),
				Text = studentTests.Question3.Text
			}
			);
			Questions.Add(new OngoingQuestion()
			{
				Id = studentTests.Q5Id,
				Argument = GetSelectedArgument(studentTests.Q5Answer),
				Answers = Answers(studentTests.Q5Answer, studentTests.Question4),
				Text = studentTests.Question4.Text
			}
			);
			Questions.Add(new OngoingQuestion()
			{
				Id = studentTests.Q6Id,
				Argument = GetSelectedArgument(studentTests.Q6Answer),
				Answers = Answers(studentTests.Q6Answer, studentTests.Question5),
				Text = studentTests.Question5.Text
			}
			);
			Questions.Add(new OngoingQuestion()
			{
				Id = studentTests.Q7Id,
				Argument = GetSelectedArgument(studentTests.Q7Answer),
				Answers = Answers(studentTests.Q7Answer, studentTests.Question6),
				Text = studentTests.Question6.Text
			}
			);
			Questions.Add(new OngoingQuestion()
			{
				Id = studentTests.Q8Id,
				Argument = GetSelectedArgument(studentTests.Q8Answer),
				Answers = Answers(studentTests.Q8Answer, studentTests.Question7),
				Text = studentTests.Question7.Text
			}
			);
			Questions.Add(new OngoingQuestion()
			{
				Id = studentTests.Q9Id,
				Argument = GetSelectedArgument(studentTests.Q9Answer),
				Answers = Answers(studentTests.Q9Answer, studentTests.Question8),
				Text = studentTests.Question8.Text
			}
			);
			Questions.Add(new OngoingQuestion()
			{
				Id = studentTests.Q10Id,
				Argument = GetSelectedArgument(studentTests.Q10Answer),
				Answers = Answers(studentTests.Q10Answer, studentTests.Question9),
				Text = studentTests.Question9.Text
			}
			);
		}

		private string GetSelectedArgument(string p)
		{

			var spl = p.Split('|');

			if (spl.Length == 2)
			{
				return spl[1];
			}
			return string.Empty;
		}

		private List<OngoingAnswer> Answers(string p, Question question)
		{
			var spl = p.Split('|');
			var selected = new List<Guid>();
			if (spl.Length == 2)
			{
				selected = spl[0].Split(',').Select(a => (new Guid(a))).ToList();
			}

			return question.Answers.Select(ans =>
			new OngoingAnswer
			{
				Id = ans.Id,
				IsSelected = selected.Contains(ans.Id),
				Text = ans.Text
			}).ToList();
		}

		public string Username { get; set; }
		public Guid TestId { get; set; }

		public int Duration { get; set; }

		public List<OngoingQuestion> Questions { get; set; }
	}
}
