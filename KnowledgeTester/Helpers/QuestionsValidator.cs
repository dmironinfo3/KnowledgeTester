using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;
using KT.ServiceInterfaces;

namespace KnowledgeTester.Helpers
{
	public class QuestionsValidatorAttribute : ValidationAttribute, IClientValidatable
	{
		private readonly string _subcategory;

		public QuestionsValidatorAttribute(string subcategory)
		{
			this._subcategory = subcategory;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var subcategoryInfo = validationContext.ObjectType.GetProperty(this._subcategory);

			var subcategoryId = subcategoryInfo.GetValue(validationContext.ObjectInstance, null);

			if (!(value is int))
			{
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
			}

			if (!(subcategoryId is Guid))
			{
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
			}

			var questions = ServicesFactory.GetService<IKtQuestionsService>()
				.GetBySubcategory((Guid)subcategoryId).Count();

			return questions < (int)value ?
			new ValidationResult(FormatErrorMessage(validationContext.DisplayName))
			: ValidationResult.Success;
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, 
			ControllerContext context)
		{
			return new List<ModelClientValidationRule>();
		}
	}

}

