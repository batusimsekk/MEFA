using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MefaOzelEgitim.DataAccess.Seed
{
    public class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = {"Admin","User"};
            foreach(var roleName in roleNames)
            {
                if(!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // ! admin giriş bilgileri
            string adminEmail = "admin@mefa.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if(adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createAdmin = await userManager.CreateAsync(newAdmin,"Admin123!");
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin,"Admin");
                    await userManager.AddClaimAsync(newAdmin,new System.Security.Claims.Claim("FirstName","Sistem"));
                    await userManager.AddClaimAsync(newAdmin,new System.Security.Claims.Claim("LastName","Yöneticisi"));
                }
            }
        }
    }
}