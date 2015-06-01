using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KT.DB.Objects
{
	public class OngoingAnswer
	{
		public string Text { get; set; }
		public bool IsSelected { get; set; }
		public Guid Id { get; set; }
	}
}
