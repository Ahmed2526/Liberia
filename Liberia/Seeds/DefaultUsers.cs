using DAL.Consts;
using Microsoft.AspNetCore.Identity;

namespace Liberia.Seeds
{
	public static class DefaultUsers
	{
		public static async Task SeedAdminUser(UserManager<ApplicationUser> _userManager)
		{
			var user = await _userManager.FindByEmailAsync("admin@gmail.com");
			if (user is null)
			{
				var adminUser = new ApplicationUser()
				{
					UserName = "Admin22",
					Email = "admin@gmail.com",
					FullName = "admin",
					EmailConfirmed = true,
					PhoneNumber = "01551759541",
					CreatedOn = DateTime.Now,
					IsActive = true
				};
				await _userManager.CreateAsync(adminUser, "admin@gmail.comA1");

				await _userManager.AddToRoleAsync(adminUser, Roles.Admin);
				await _userManager.AddToRoleAsync(adminUser, Roles.Archive);
				await _userManager.AddToRoleAsync(adminUser, Roles.Reception);
			}
		}
	}
}
