﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using KT.DB;

namespace KnowledgeTester.Models
{
	public class SubcategoryModel
	{
		public SubcategoryModel(Subcategory subcategory, string categoryName)
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