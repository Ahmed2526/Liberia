using DAL.Models.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(NationalId), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class Subscriber : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }

        [MaxLength(100)]
        public string NationalId { get; set; } = null!;

        [MaxLength(100)]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(100)]
        public string Email { get; set; } = null!;
        public bool HasWhatsapp { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; } = null!;

        [MaxLength(500)]
        public string? Thumbnail { get; set; } = null!;

        public int? GovernorateId { get; set; }
        public Governorate? Governorate { get; set; }

        public int? AreaId { get; set; }
        public Area? Area { get; set; }

        [MaxLength(20)]
        public string ZipCode { get; set; } = null!;

        [MaxLength(250)]
        public string Address { get; set; } = null!;

        public bool IsBlocked { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }

    }
}
