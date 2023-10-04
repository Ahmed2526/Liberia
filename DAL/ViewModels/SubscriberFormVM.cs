using DAL.Consts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace DAL.ViewModels
{
    public class SubscriberFormVM
    {
        public string? Id { get; set; }

        [MaxLength(100)]
        [DisplayName("First Name")]
        [RegularExpression(RegexPatterns.EnglishLettersOnlyWithSpacePattern, ErrorMessage = Errors.EnglishLettersOnly)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        [DisplayName("Last Name")]
        [RegularExpression(RegexPatterns.EnglishLettersOnlyWithSpacePattern, ErrorMessage = Errors.EnglishLettersOnly)]
        public string LastName { get; set; } = null!;

        [DisplayName("Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(100)]
        [DisplayName("National ID")]
        [RegularExpression(RegexPatterns.EgyNationalNumber, ErrorMessage = Errors.InvalidNationalNumber)]
        [Remote("checkUniqueNationalId", "Subscribers", AdditionalFields = "Id", ErrorMessage = "Subscriber with the same National ID already exist!")]
        public string NationalId { get; set; } = null!;

        [MaxLength(100)]
        [DisplayName("Phone number")]
        [RegularExpression(RegexPatterns.EgyPhonePattern, ErrorMessage = Errors.Phone)]
        [Remote("checkUniquePhoneNumber", "Subscribers", AdditionalFields = "Id", ErrorMessage = "Subscriber with the same phone number already exist!")]
        public string PhoneNumber { get; set; } = null!;

        [EmailAddress]
        [MaxLength(100)]
        [RegularExpression(RegexPatterns.EmailPattern, ErrorMessage = Errors.Email)]
        [Remote("checkUniqueEmail", "Subscribers", AdditionalFields = "Id", ErrorMessage = "Subscriber with the same email already exist!")]
        public string Email { get; set; } = null!;

        [DisplayName("Profile photo")]
        public IFormFile? ProfilePhoto { get; set; } = null!;
        public string? ProfilePath { get; set; }

        [DisplayName("Governorate")]
        public int GovernorateId { get; set; }
        public IEnumerable<SelectListItem>? Governorate { get; set; }

        [DisplayName("Area")]
        public int AreaId { get; set; }
        public IEnumerable<SelectListItem>? Area { get; set; }

        [MaxLength(20)]
        [DisplayName("Postal Code")]
        [RegularExpression(RegexPatterns.EgyPostalCode, ErrorMessage = Errors.InvalidPostalNumber)]
        public string ZipCode { get; set; } = null!;

        [MaxLength(250)]
        public string Address { get; set; } = null!;

        [DisplayName("Has WhatsApp?")]
        public bool HasWhatsApp { get; set; }

    }
}
