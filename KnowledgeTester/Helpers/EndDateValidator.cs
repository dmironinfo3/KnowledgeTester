using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KnowledgeTester.Helpers
{
	public class EndDateValidatorAttribute : ValidationAttribute, IClientValidatable
	{
		private readonly string _startDate;
		private readonly string _duration;

		public EndDateValidatorAttribute(string startdate, string duration)
		{
			this._startDate = startdate;
			this._duration = duration;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var startDateInfo = validationContext.ObjectType.GetProperty(this._startDate);
			var durationInfo = validationContext.ObjectType.GetProperty(this._duration);



			var startDate = startDateInfo.GetValue(validationContext.ObjectInstance, null);
			var duration = durationInfo.GetValue(validationContext.ObjectInstance, null);

			if (!(value is DateTime))
			{
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
			}

			if (!(duration is int))
			{
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
			}

			if (!(startDate is DateTime))
			{
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
			}

			if (((DateTime) value) < ((DateTime)startDate).AddMinutes((int)duration))
			{
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
			}

			return ValidationResult.Success;
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			return new List<ModelClientValidationRule>();
		}
	}
}