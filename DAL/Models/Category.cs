using DAL.Models.BaseModels;

namespace DAL.Models
{
	public class Category : BaseEntity
	{
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		public ICollection<BookCategory> Books { get; set; } = new List<BookCategory>();
	}
}
