using DAL.Models.BaseModels;

namespace DAL.Models
{
	public class Author : BaseEntity
	{

		public Author()
		{
			Books = new HashSet<Book>();
		}

		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;

		public ICollection<Book> Books { get; set; }

	}
}
