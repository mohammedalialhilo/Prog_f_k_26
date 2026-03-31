using System.Text.Json;
using eShop.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace eShop.Data;

public class SeedDatabase(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
{

    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public async Task InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EShopContext>();

        context.Database.Migrate();


        await SeedUsers();
        await SeedSupplier(context);
        await SeedProducts(context);
    }

    public async Task SeedUsers()
    {
        

        var role = await roleManager.FindByNameAsync("Admin");
        if (role is null)
        {
            await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        }

        var admin = await userManager.FindByNameAsync("Admin");
        if (admin is null)
        {
            var user = new User
            {
                Email = "admin@mail.com",
                UserName = "Admin",
                FirstName = "Admin",
                LastName = "Adminsson"
            };
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }

    public async Task SeedSupplier(EShopContext context)
    {
        if (context.Suppliers.Any())return;

        var json = File.ReadAllText("Data/Json/suppliers.json");
        var suppliers = System.Text.Json.JsonSerializer.Deserialize<List<Supplier>>(json, options);

        if (suppliers is not null && suppliers.Count > 0)
        {
            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();
        }


    }


    public async Task SeedProducts(EShopContext context)
    {
        if (context.Products.Any())return;

        var json = File.ReadAllText("Data/Json/products.json");
        var products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json, options);

        if (products is not null && products.Count > 0)
        {
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }


}
