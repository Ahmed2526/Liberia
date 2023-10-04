using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DAL.ViewModels
{
    public class BookFormVM
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [MinLength(3, ErrorMessage = "Minimum Length is 3 char")]
        [Remote("checkUnique", "Books", AdditionalFields = "Id,AuthorId", ErrorMessage = "Book With Same Title And Author Already Exist!")]
        public string Title { get; set; } = string.Empty;
        [Display(Name = "Author")]
        [Remote("checkUnique", "Books", AdditionalFields = "Id,Title", ErrorMessage = "Book With Same Title And Author Already Exist!")]
        public int AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Authors { get; set; }

        [MaxLength(200)]
        public string Publisher { get; set; } = string.Empty;
        [Display(Name = "Publishing Date")]
        public DateTime PublishingDate { get; set; } = DateTime.Now;

        public IFormFile? ImageUrl { get; set; }
        public string? ImagePath { get; set; }
        public string? ThumbNailPath { get; set; }

        [MaxLength(50)]
        public string Hall { get; set; } = string.Empty;

        [Display(Name = "Is Available For Rental?")]
        public bool IsAvailableForRental { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        [Display(Name = "Categories")]
        public List<int> SelectedCategories { get; set; } = new();
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}
