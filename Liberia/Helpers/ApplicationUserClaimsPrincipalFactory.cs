using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Liberia.Helpers
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory
            (UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var id = await base.GenerateClaimsAsync(user);

            id.AddClaim(new Claim(ClaimTypes.Surname, user.FullName));
            id.AddClaim(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));

            return id;
        }
    }
}
