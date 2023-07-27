using DAL.Models.BaseModels;

namespace DAL.Models
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; }
    }
}
