using DAL.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DAL.ViewModels
{
	public class UserFormVM
	{
		public string? UserId { get; set; }

		[Display(Name = "Full Name")]
		[RegularExpression(RegexPatterns.EnglishLettersOnlyWithSpacePattern, ErrorMessage = Errors.EnglishLettersOnly)]
		public string FullName { get; set; } = string.Empty;

		[Display(Name = "User Name")]
		[Remote("checkUnique", "Users", AdditionalFields = "UserId", ErrorMessage = "UserName already taken")]
		[RegularExpression(RegexPatterns.EnglishLettersAndNumbersOnlyPattern, ErrorMessage = Errors.EnglishLettersAndNumbers)]
		public string UserName { get; set; } = string.Empty;

		[Required]
		[EmailAddress]
		[Remote("checkUnique", "Users", AdditionalFields = "UserId", ErrorMessage = "Email already registered")]
		[RegularExpression(RegexPatterns.EmailPattern, ErrorMessage = Errors.Email)]
		public string Email { get; set; } = string.Empty;

		[RegularExpression(RegexPatterns.EgyPhonePattern, ErrorMessage = Errors.Phone)]
		public string PhoneNumber { get; set; } = string.Empty;

		[Required]
		[StringLength(100, ErrorMessage = Errors.PasswordMinValue, MinimumLength = 6)]
		[DataType(DataType.Password)]
		[RegularExpression(RegexPatterns.PasswordPattern, ErrorMessage = Errors.PasswordPattern)]
		public string Password { get; set; } = string.Empty;

		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare("Password", ErrorMessage = Errors.PasswordMisMatch)]
		public string ConfirmPassword { get; set; } = string.Empty;

		public List<string> Roles { get; set; } = new();
		public IEnumerable<SelectListItem>? SelectedRoles { get; set; }

	}
}
