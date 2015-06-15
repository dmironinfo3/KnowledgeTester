using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KT.DTOs.Objects;
using KnowledgeTester.Helpers;
using WebGrease.Css.ImageAssemblyAnalysis.LogModel;

namespace KnowledgeTester.Models
{
	public class TestModel
	{
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required(ErrorMessage = "Start time is required!")]
		[StartDateValidator(ErrorMessage = "The test's earliest start time is within an hour!")]
		public DateTime? StartDate { get; set; }

		[Required(ErrorMessage = "Subcategory is required!")]
		public Guid SubcategoryId { get; set; }

		[Required(ErrorMessage = "Duration is required!")]
		[Range(5, 120, ErrorMessage = "Duration should be between 5 and 120 minutes")]
		public int? Duration { get; set; }

		[Required(ErrorMessage = "No. of questions is required!")]
		[Range(5, 120, ErrorMessage = "No. of questions should be between 5 and 120 minutes")]
		[QuestionsValidator("SubcategoryId", ErrorMessage = "Not enough questions added to the subcategory")]
		public int? Questions { get; set; }

		[Required(ErrorMessage = "End Time is required!")]
		[EndDateValidator("StartDate", "Duration",
			ErrorMessage = "End time shouldn't be earlier than start time plus duration")]
		public DateTime? EndDate { get; set; }

		public List<SubcategoryModel> Subcategories { get; set; }

		public TestModel(TestDto test)
		{
			Id = test.Id;
			Name = test.Name;
			StartDate = test.StartTime;
			EndDate = test.EndTime;
			Duration = test.Duration;
			Questions = test.Questions;
			SubcategoryId = test.SubcategoryId;
		}

		public TestModel()
		{
			StartDate = null;
			EndDate = null;
			Duration = null;
		}
	}


}