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
			var date = (DateTime)value;

			return date.AddHours(1) < DateTime.Now;
		}
	}
}