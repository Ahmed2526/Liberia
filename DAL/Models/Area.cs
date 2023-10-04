using DAL.Models.BaseModels;

namespace DAL.Models
{
    public class Area : BaseEntity
    {
        [MaxLength(100)]
        public string NameEn { get; set; } = null!;

        [MaxLength(100)]
        public string NameAr { get; set; } = null!;

        public int GovernorateId { get; set; }

        public Governorate? Governorate { get; set; }
    }
}
