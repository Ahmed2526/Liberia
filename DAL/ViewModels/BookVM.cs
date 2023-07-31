using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IFormFile? ImageUrl { get; set; }

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
