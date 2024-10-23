using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIS.Extensions
{
    public static class UserMangerExtensions
    {
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManger,ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user =await userManger.Users.Include(U=>U.Address).FirstOrDefaultAsync(U=>U.Email ==email);
            return user;
        }
    }
}
