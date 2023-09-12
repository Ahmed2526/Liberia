namespace DAL.ViewModels
{
    public class BookCopyFormVM
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        [Display(Name = "Edition Number")]
        public int EditionNumber { get; set; }

        [Display(Name = "Is Available For Rental ?")]
        public bool IsAvailableForRental { get; set; }
    }
}
