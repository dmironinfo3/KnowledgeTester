using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using KnowledgeTester.Models;

namespace KnowledgeTester.Helpers
{
	public class StartDateValidator : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (!(value is DateTime))
			{
				return false;
			}

			var date = (DateTime)value;

			return date >= DateTime.Now.AddHours(1);
		}
	}
}