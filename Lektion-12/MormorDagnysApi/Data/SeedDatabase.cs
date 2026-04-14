using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MormorDagnysApi.Entities;

namespace MormorDagnysApi.Data;

public class SeedDatabase
{
    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MormorDagnysContext>();

        await context.Database.MigrateAsync();
        await SeedBakeryData(context, app);
    }

    private static async Task SeedBakeryData(MormorDagnysContext context, WebApplication app)
    {
        if (await context.Suppliers.AnyAsync() ||
            await context.Products.AnyAsync() ||
            await context.SupplierProducts.AnyAsync())
        {
            return;
        }

        var suppliers = ReadSeedFile<List<Supplier>>(app, "suppliers.json");
        var products = ReadSeedFile<List<Product>>(app, "products.json");
        var supplierProducts = ReadSeedFile<List<SupplierProduct>>(app, "supplierProducts.json");

        await context.Suppliers.AddRangeAsync(suppliers);
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        await context.SupplierProducts.AddRangeAsync(supplierProducts);
        await context.SaveChangesAsync();
    }

    private static T ReadSeedFile<T>(WebApplication app, string fileName)
    {
        var filePath = Path.Combine(app.Environment.ContentRootPath, "Data", "Json", fileName);
        var json = File.ReadAllText(filePath);

        return JsonSerializer.Deserialize<T>(json, options);
    }
}
