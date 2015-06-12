using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using KT.DB;
using KT.DTOs.Objects;
using KnowledgeTester.Helpers;

namespace KnowledgeTester.Models
{
	public class TestModel
	{
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required(ErrorMessage = "Start time is required!")]
		[AnswerValidator(ErrorMessage = "The test's earliest start time is within an hour!")]
		public DateTime? StartDate { get; set; }

		[Required(ErrorMessage = "Subcategory is required!")]
		public Guid SubcategoryId { get; set; }

		[Required(ErrorMessage = "Duration is required!")]
		[Range(5, 120, ErrorMessage = "Duration should be between 5 and 120 minutes")]
		public int? Duration { get; set; }

		public DateTime? EndDate { get; set; }

		public List<SubcategoryModel> Subcategories { get; set; }

		public TestModel(TestDto test)
		{
			Id = test.Id;
			Name = test.Name;
			StartDate = test.StartTime;
			EndDate = test.EndTime;
			Duration = test.Duration;
			SubcategoryId = test.Subcategory.Id;
		}

		public TestModel()
		{
			StartDate = null;
			EndDate = null;
			Duration = null;
		}
	}
}