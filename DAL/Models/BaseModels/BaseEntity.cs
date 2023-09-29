namespace DAL.Models.BaseModels
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifiedOn { get; set; }

    }
}
