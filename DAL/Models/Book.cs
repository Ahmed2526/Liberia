using DAL.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Book : BaseEntity
    {
        public Book()
        {
            Categories = new HashSet<Category>();
        }

        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [ForeignKey(nameof(AuthorId))]
        public Author? Author { get; set; }
        public int AuthorId { get; set; }

        [MaxLength(200)]
        public string Publisher { get; set; } = string.Empty;
        public DateTime PublishingDate { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Hall { get; set; } = string.Empty;

        public bool IsAvailableForRental { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public ICollection<Category> Categories { get; set; }

    }
}
