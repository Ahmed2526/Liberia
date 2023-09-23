using DAL.Models.BaseModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
	[Index(nameof(AuthorId), nameof(Title), IsUnique = true)]
	public class Book : BaseEntity
	{
		[MaxLength(100)]
		public string Title { get; set; } = string.Empty;

		[ForeignKey(nameof(AuthorId))]
		public Author? Author { get; set; }
		public int AuthorId { get; set; }

		[MaxLength(200)]
		public string Publisher { get; set; } = string.Empty;
		public DateTime PublishingDate { get; set; }
		public string ImageName { get; set; } = string.Empty;
		public string ThumbNail { get; set; } = string.Empty;

		[MaxLength(50)]
		public string Hall { get; set; } = string.Empty;

		public bool IsAvailableForRental { get; set; }

		[MaxLength(2000)]
		public string Description { get; set; } = string.Empty;

		public ICollection<BookCategory> Categories { get; set; } = new List<BookCategory>();
		public ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();
	}
}
