using DAL.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DAL.ViewModels
{
	public class EditUserVM
	{
		public string UserId { get; set; } = null!;

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

		public List<string> Roles { get; set; }
		public IEnumerable<SelectListItem>? SelectedRoles { get; set; }

	}
}
