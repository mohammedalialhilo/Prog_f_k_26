using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using eShop.Services;

namespace eShop.Data;

public class SeedDatabase(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
{
    public async Task InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await SeedData(scope.ServiceProvider.GetRequiredService<EShopContext>());


    }
    public async Task SeedData(EShopContext context)
    {
        context.Database.Migrate();
        var role = await roleManager.FindByNameAsync("Admin");
        if (role == null)
        {
            await roleManager.CreateAsync(new IdentityRole(Name = "Admin"));
        }

        var admin = await userManager.FindByNameAsync("Admin");
        if (admin == null)
        {
            var user = new User
            {
                Email = "admin@example.com",
                UserName = "Admin",
                FirstName = "Admin",
                LastName = "Admin"
            };
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Admin");

        }
    }


}
