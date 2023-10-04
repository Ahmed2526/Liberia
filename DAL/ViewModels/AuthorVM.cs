using DAL.Consts;
using Microsoft.AspNetCore.Mvc;

namespace DAL.ViewModels
{
    public class AuthorVM
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [MinLength(3, ErrorMessage = "Minimum Length is 3 char")]
        [Remote("checkUnique", "Authors", AdditionalFields = "Id", ErrorMessage = "Author Already Exist!")]
        [RegularExpression(RegexPatterns.EnglishLettersandDotPattern, ErrorMessage = Errors.EnglishLettersOnly)]
        public string Name { get; set; } = string.Empty;
    }
}
