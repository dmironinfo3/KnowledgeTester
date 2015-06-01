using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace KnowledgeTester.Models
{
	public class CategoryModel
	{
		public CategoryModel(KT.DB.Category cat)
		{
			Name = cat.Name;
			Id = cat.Id;
		}

		public CategoryModel()
		{
			
		}

		[Required]
		public string Name { get; set; }
		public Guid Id { get; set; }

		public List<SubcategoryModel> Subcategories { get; set; }
	}
}
