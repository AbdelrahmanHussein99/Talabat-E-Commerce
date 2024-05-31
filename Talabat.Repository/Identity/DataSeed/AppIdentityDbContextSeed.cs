using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity.DataSeed
{
    public static  class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0) 
            {
                var user = new AppUser()
                {
                    DisplayName = "Abdelrahman Hussein",
                    Email = "abdelrahmanhussain123@gmail.com",
                    UserName = "abdelrahman.Hussein",
                    PhoneNumber = "0123456768"
                };

                await _userManager.CreateAsync(user, "Pa$$W0rd");
            } 
        }
    }
}
