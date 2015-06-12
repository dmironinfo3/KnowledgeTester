using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KnowledgeTester.Models
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Username is required")]
		[StringLength(128, ErrorMessage = "Username too long!")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[StringLength(256, ErrorMessage = "Password too long!")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Password confirmation is required")]
		[Compare("Password", ErrorMessage = "Passwords don't match")]
		public string PasswordConfirm { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[StringLength(256, ErrorMessage = "Email too long!")]
		[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Email confirmation is required")]
		[Compare("Email", ErrorMessage = "Emails don't match")]
		public string EmailConfirm { get; set; }

		[Required(ErrorMessage = "Password hint is required")]
		[StringLength(1024, ErrorMessage = "Password hint too long!")]
		public string PassHint { get; set; }

		[Required(ErrorMessage = "First name is required")]
		[StringLength(1024, ErrorMessage = "First name too long!")]
		public string Firstname { get; set; }

		[Required(ErrorMessage = "Last name is required")]
		[StringLength(1024, ErrorMessage = "First name too long!")]
		public string LastName { get; set; }
	}
}