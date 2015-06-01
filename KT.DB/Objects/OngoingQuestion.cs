using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KT.DB.Objects
{
	public class OngoingQuestion
	{
		public List<OngoingAnswer> Answers { get; set; }
		public string Text { get; set; }
		public string Argument { get; set; }
		public Guid Id { get; set; }
	}
}
