using AMS_data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Asset_management_Web_Core.Seeders
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var username = "admin";
            var password = "Halco12345!!!";

            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = username,
                    Email = "admin@local.local",
                    EmailConfirmed = true,
                    Ime = "System",
                    Prezime = "Admin",
                    Aktivan = true
                };

                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}