using DAL.Consts;

namespace DAL.ViewModels
{
    public class BookCopyFormVM
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        [Display(Name = "Edition Number")]
        [RegularExpression(RegexPatterns.NumericsonlyPattern, ErrorMessage = Errors.NumbersOnly)]
        public int EditionNumber { get; set; }

        [Display(Name = "Is Available For Rental ?")]
        public bool IsAvailableForRental { get; set; }
    }
}
