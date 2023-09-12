using Liberia.Consts;
using Microsoft.AspNetCore.Identity;

namespace Liberia.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUser(UserManager<ApplicationUser> _userManager)
        {
            var user = await _userManager.FindByNameAsync("Admin");
            if (user is null)
            {
                var adminUser = new ApplicationUser()
                {
                    UserName = "Admin",
                    Email = "admin@gmail.com",
                    FullName = "admin",
                    EmailConfirmed = true,
                };
                await _userManager.CreateAsync(adminUser, "admin@gmail.comA1");

                await _userManager.AddToRoleAsync(adminUser, Roles.Admin);
                await _userManager.AddToRoleAsync(adminUser, Roles.Archive);
                await _userManager.AddToRoleAsync(adminUser, Roles.Reception);
            }
        }
    }
}
