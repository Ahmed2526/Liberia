using DAL.Consts;

namespace DAL.ViewModels
{
    public class BookCopyVM
    {
        public int Id { get; set; }
        public string BookTitle { get; set; } = null!;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public bool IsAvailableForRental { get; set; }

        [RegularExpression(RegexPatterns.NumericsonlyPattern, ErrorMessage = Errors.NumbersOnly)]
        public int EditionNumber { get; set; }
        public int SerialNumber { get; set; }
    }
}
