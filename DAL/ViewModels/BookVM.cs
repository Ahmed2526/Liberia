using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DAL.ViewModels
{
    public class BookVM
    {
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Display(Name ="Author")]
        public int AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Authors { get; set; }

        [MaxLength(200)]
        public string Publisher { get; set; } = string.Empty;
        [Display(Name = "Publishing Date")]
        public DateTime PublishingDate { get; set; } = DateTime.Now;

        [Required]
        public IFormFile ImageUrl { get; set; }

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
