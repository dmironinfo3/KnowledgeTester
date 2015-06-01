using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnowledgeTester.Models
{
	public class TakeAnswerModel
	{
		public TakeAnswerModel()
		{
			
		}

		public string Text { get; set; }
		public bool IsSelected { get; set; }
		public Guid Id { get; set; }
	}
}