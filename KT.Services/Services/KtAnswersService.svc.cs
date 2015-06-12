using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using KT.DB;
using KT.ServiceInterfaces;

namespace KT.Services.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	public class KtAnswersService : IKtAnswersService
	{
		public void AddEmpyFor(Guid id)
		{
			using (var db = new KTEntities())
			{
				var ans = new Answer()
				{
					Id = Guid.NewGuid(),
					Text = string.Empty,
					IsCorrect = false,
					QuestionId = id
				};

				db.Answers.AddObject(ans);
				db.SaveChanges();
			}
		}

		public void Delete(Guid id)
		{
			using (var db = new KTEntities())
			{
				var ans = db.Answers.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				if (ans != null)
				{
					db.Answers.DeleteObject(ans);
					db.SaveChanges();
				}
			}
		}

		public void Save(Guid id, string text, bool isCorrect)
		{
			using (var db = new KTEntities())
			{
				var ans = db.Answers.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				if (ans != null)
				{
					ans.Text = text;
					ans.IsCorrect = isCorrect;
					db.SaveChanges();
				}
			}
		}

		public void SaveTestAnswer(Guid ansId, bool selected)
		{
			using (var db = new KTEntities())
			{
				var answer =
					db.GeneratedAnswers.DefaultIfEmpty(null).
					FirstOrDefault(a => a.Id == ansId);

				if (answer != null)
				{
					answer.IsSelected = selected;

					db.SaveChanges();
				}
			}
		}
	}
}
