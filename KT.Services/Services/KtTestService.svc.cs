using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using KT.DB;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;
using KT.Services.Mappers;

namespace KT.Services.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KtTestService" in code, svc and config file together.
	public class KtTestService : IKtTestService
	{
		public IEnumerable<TestDto> GetAll()
		{
			using (var db = new KTEntities())
			{
				return (new TestsMapper()).Map(db.Tests.Include("Subcategory").Include("Students").ToList());
			}
		}

		public void Delete(Guid id)
		{
			using (var db = new KTEntities())
			{
				var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

				db.Tests.DeleteObject(test);
				db.SaveChanges();
			}
		}

		public TestDto GetById(Guid testId)
		{
			using (var db = new KTEntities())
			{
				var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == testId);
				return (new TestsMapper()).Map(test);
			}
		}

		public Guid Save(string name, DateTime? startDate, DateTime? endDate, int? duration, Guid subcategoryId, Guid? id = null)
		{
			using (var db = new KTEntities())
			{
				if (id.HasValue && !id.Equals(Guid.Empty))
				{
					var test = db.Tests.DefaultIfEmpty(null).FirstOrDefault(a => a.Id == id);

					if (test != null)
					{
						test.Name = name;
						if (startDate != null) test.StartDate = startDate.Value;
						test.EndDate = endDate.Value;
						if (duration != null) test.MinutesDuration = duration.Value;
						test.SubcategoryId = subcategoryId;
					}
					db.SaveChanges();
					return test.Id;
				}
				else
				{
					var test = new Test { Name = name, Id = Guid.NewGuid(), SubcategoryId = subcategoryId, StartDate = startDate.Value, EndDate = endDate.Value, MinutesDuration = duration.Value };
					db.Tests.AddObject(test);
					db.SaveChanges();
					return test.Id;

				}
			}
		}

		public IEnumerable<TestDto> GetAllUpcoming(string username)
		{
			using (var db = new KTEntities())
			{
				var tst = new List<Test>();

				var tests = db.Users.Include("Tests").First(a => a.Username == username).Tests.ToList();

				foreach (var test in tests)
				{
					if ((new KtUserTestsService()).IsValidInProgress(test.Id, username))
					{
						tst.Add(db.Tests.Include("Students").Include("Subcategory").First(a => a.Id == test.Id));
					}
				}
				return (new TestsMapper()).Map(tst);
			}
		}

		public IEnumerable<TestDto> GetFinishedTests(string username)
		{
			using (var db = new KTEntities())
			{
				var tests = db.GeneratedTests.Include("Test").Where(a => a.Username == username
				&& a.IsFinished).ToList();

				return (new GeneratedTestsMapper()).Map(tests).Select(a=>a.Test);
			}
		}
	}
}
