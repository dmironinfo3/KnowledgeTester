using System.Collections.Generic;

namespace KnowledgeTester.Models
{
	public class AdminPanelModel
	{
		public string Category_NewName { get; set; }

		public string Subcategory_NewName { get; set; }
		public string Subcategory_Cat { get; set; }

		public IEnumerable<CategoryModel> Categories { get; set; }
	}
}