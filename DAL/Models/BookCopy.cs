using DAL.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class BookCopy : BaseEntity
    {
        public BookCopy()
        {
            var random = new Random();
            int val = random.Next(1000000, 9999999);
            SerialNumber = val;
        }

        [ForeignKey("BookId")]
        public Book? Book { get; set; }
        public int BookId { get; set; }
        public bool IsAvailableForRental { get; set; }
        public int EditionNumber { get; set; }
        public int SerialNumber { get; set; }
    }
}
