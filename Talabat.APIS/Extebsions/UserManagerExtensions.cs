using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIS.Extebsions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal _user)
        {
            var userEmail = _user.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(A => A.Address).FirstOrDefaultAsync(U=>U.Email==userEmail);

            return user;
        }
    }
}
