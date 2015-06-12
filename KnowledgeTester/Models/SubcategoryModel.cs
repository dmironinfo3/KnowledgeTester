using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using KT.DB;
using KT.DTOs.Objects;

namespace KnowledgeTester.Models
{
	public class SubcategoryModel
	{
		public SubcategoryModel(SubcategoryDto subcategory, string categoryName)
		{
			Name = subcategory.Name;
			Id = subcategory.Id;
			CategoryName = categoryName;
		}

		public SubcategoryModel(string categoryName)
		{
			CategoryName = categoryName;
		}

		[Required]
		public string Name { get; set; }
		public Guid Id { get; set; }

		public string CategoryName { get; set; }
	}
}
