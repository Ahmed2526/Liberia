using DAL.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class PasswordVM
    {
        public string UserId { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; } = null!;
        [Required]
        [StringLength(100, ErrorMessage = Errors.PasswordMinValue, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression(RegexPatterns.PasswordPattern, ErrorMessage = Errors.PasswordPattern)]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = Errors.PasswordMisMatch)]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
