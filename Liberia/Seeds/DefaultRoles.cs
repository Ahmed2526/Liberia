using DAL.Consts;
using Microsoft.AspNetCore.Identity;

namespace Liberia.Seeds
{
	public static class DefaultRoles
	{
		public static async Task SeedRoles(RoleManager<IdentityRole> _roleManager)
		{
			if (!_roleManager.Roles.Any())
			{
				await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
				await _roleManager.CreateAsync(new IdentityRole(Roles.Archive));
				await _roleManager.CreateAsync(new IdentityRole(Roles.Reception));
			}
		}
	}
}
