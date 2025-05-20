using Microsoft.AspNetCore.Identity;
using social_media.Data.Constants;
using social_media.Data.Models;

namespace social_media.Data.Helpers
{
    public class DbInitializer
    {
        public static async Task SeedUserAndRoleAsync(UserManager<User>  usermanager , RoleManager<IdentityRole<int>> rolemanager)
        {
        //    var test = rolemanager.Roles.Any();
        //    if (!rolemanager.Roles.Any())
        //    {
        //        foreach(var role in AppRoles.All)
        //        {
        //            if (!await rolemanager.RoleExistsAsync(role))
        //            {
        //                await rolemanager.CreateAsync(new IdentityRole<int>(role));
        //            }
        //        }
        //    }

        //    if (!usermanager.Users.Any(n => !string.IsNullOrEmpty(n.Email)))
        //    {
        //        var userpassword = "Mankiam2004@";
        //        var newuser = new User()
        //        {
        //            UserName = "mamad",
        //            Email = "mamad@email.com",
        //            FullName = "mohammad hosein",
        //            EmailConfirmed = true,
        //            ProfilePictureUrl = "/images/ee5d2359-d067-44cd-afdb-669775986903.jpg"
        //        };
        //        var result = await usermanager.CreateAsync(newuser , userpassword);

        //        if (result.Succeeded)
        //        {
        //            await usermanager.AddToRoleAsync(newuser, AppRoles.User);
        //        }

        //        var newAdmin = new User()
        //        {
        //            UserName = "Admin",
        //            Email = "Admin@email.com",
        //            FullName = "Admin",
        //            EmailConfirmed = true,
        //            ProfilePictureUrl = "/images/ee5d2359-d067-44cd-afdb-669775986903.jpg"
        //        };
        //        var resultAdmin = await usermanager.CreateAsync(newAdmin, userpassword);

        //        if (resultAdmin.Succeeded)
        //        {
        //            await usermanager.AddToRoleAsync(newAdmin, AppRoles.Admin);
        //        }
        //    }
        }
       
    }
}
