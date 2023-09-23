namespace DAL.ViewModels
{
	public class UserVM
	{
		public string UserId { get; set; } = string.Empty;

		[Display(Name = "Full Name")]
		public string FullName { get; set; } = string.Empty;

		public string UserName { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string PhoneNumber { get; set; } = string.Empty;

		public DateTime CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }
		public bool IsActive { get; set; }

		//[Display(Name = "User Role")]
		//public string UserRole { get; set; } = string.Empty;
	}
}
