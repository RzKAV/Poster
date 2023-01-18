using Microsoft.AspNetCore.Identity;
using Poster.Domain.Entities;

namespace Poster.Infrastructure;

public static class AppDbContextSeed
{
    public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminUserName = "Admin";
        string adminEmail = "admin@gmail.com";
        string password = "Prikol123";
        if (await roleManager.FindByNameAsync("admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("admin"));
        }
        if (await roleManager.FindByNameAsync("user") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("user"));
        }
        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            var admin = new AppUser { Email = adminEmail, UserName = adminUserName };
            IdentityResult result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}