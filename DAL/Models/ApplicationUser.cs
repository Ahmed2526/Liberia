using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
	[Index(nameof(Email), IsUnique = true)]
	[Index(nameof(UserName), IsUnique = true)]
	public class ApplicationUser : IdentityUser
	{
		[MaxLength(150)]
		public string FullName { get; set; } = null!;
		public bool IsActive { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public string? CreatedById { get; set; }
		public string? ModifiedById { get; set; }

	}
}
