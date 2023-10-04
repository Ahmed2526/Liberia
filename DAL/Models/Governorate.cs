using DAL.Models.BaseModels;

namespace DAL.Models
{
    public class Governorate : BaseEntity
    {
        [MaxLength(100)]
        public string NameEn { get; set; } = null!;

        [MaxLength(100)]
        public string NameAr { get; set; } = null!;

        public ICollection<Area> Areas { get; set; } = new List<Area>();

    }
}
