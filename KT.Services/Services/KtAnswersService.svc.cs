using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using KT.DB;
using KT.DB.CRUD;
using KT.ServiceInterfaces;

namespace KT.Services.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	public class KtAnswersService : IKtAnswersService
	{
		private static readonly ICrud<Answer> Repository = CrudFactory<Answer>.Get();

		public void AddEmpyFor(Guid id)
		{
			var ans = new Answer
			{
				Id = Guid.NewGuid(),
				Text = string.Empty,
				IsCorrect = false,
				QuestionId = id
			};

			Repository.Create(ans);
		}

		public void Delete(Guid id)
		{
			Repository.Delete(a => a.Id == id);
		}

		public void Save(Guid id, Guid questionId, string text, bool isCorrect)
		{
			var ans = Repository.Read(a => a.Id == id);

			if (ans != null)
			{
				ans.Text = text;
				ans.IsCorrect = isCorrect;
				Repository.Update(ans);
			}
			else
			{
				ans = new Answer
				{
					Id = Guid.NewGuid(),
					QuestionId = questionId,
					Text = text,
					IsCorrect = isCorrect
				};

				Repository.Create(ans);
			}
		}

		public void SaveTestAnswer(Guid ansId, bool selected)
		{
			var rep = CrudFactory<GeneratedAnswer>.Get();

			var ans = rep.Read(a => a.Id == ansId);

			if (ans != null)
			{
				ans.IsSelected = selected;
				rep.Update(ans);
			}
		}
	}
}
