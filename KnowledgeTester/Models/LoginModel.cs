using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KnowledgeTester.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Username is required")]
		[StringLength(128, ErrorMessage = "Username too long!")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[StringLength(256, ErrorMessage = "Password too long!")]
		public string Password { get; set; }

		public string PassHint { get; set; }
	}
}