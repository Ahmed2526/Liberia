using DAL.Consts;
using Microsoft.AspNetCore.Mvc;

namespace DAL.ViewModels
{
	public class CategoryVM
	{
		public int Id { get; set; }

		[Required, MaxLength(100)]
		[MinLength(3, ErrorMessage = "Minimum Length is 3 char")]
		[Remote("checkUnique", "Categories", AdditionalFields = "Id", ErrorMessage = "Category Already Exist!")]
		[RegularExpression(RegexPatterns.EnglishLettersOnlyPattern, ErrorMessage = Errors.EnglishLettersOnly)]
		public string Name { get; set; } = string.Empty;
	}
}
