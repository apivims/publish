using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Identity
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData
            (UserManager<IdentityUser> userManager,  RoleManager<IdentityRole> roleManager)
        {

        }

        public static void SeedUsers (UserManager<IdentityUser> userManager)
        {

        }

        public static void SeedRoles (RoleManager<IdentityRole> roleManager)
        {

        }
    }
}
