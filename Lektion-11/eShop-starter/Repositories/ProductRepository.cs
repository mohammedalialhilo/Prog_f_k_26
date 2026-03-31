using eShop.Data;
using eShop.DTOs.Products;
using eShop.Entities;
using Microsoft.EntityFrameworkCore;


namespace eShop.Repositories;

public class ProductRepository(EShopContext context) : IProductRepository
{


    public async Task<List<GetProductsDto>> ListAllProducts()
    {
        try
        {
            var product = await context.Products
            .Select(product => new GetProductsDto
            {
                Id = product.Id,
                Name = product.ProductName,
                ItemNumber = product.ItemNumber,
                Price = product.Price
            })
            .ToListAsync();

            if(product is null) throw new Exception("Hittade ingen product");

            return product;
        }catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }
    public async Task<GetProductDto> FindProduct(int id)
    {
        var product = await context.Products
            .Where(c => c.Id == id)
            .Select(p => new GetProductDto
            {
                Id = p.Id,
                Name = p.ProductName,
                ItemNumber = p.ItemNumber,
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl
            })
            .SingleOrDefaultAsync();
       
        return product;
    }

    public async Task<int> AddProduct(PostProductDto product)
    {
        try
        {
            Supplier supplier = await context.Suppliers.FirstOrDefaultAsync(s => s.SupplierName.ToLower().Trim() == product.SupplierName.ToLower()) ?? throw new Exception("Kunde inte hitta leverantör");
            
            Product item = new()
        {
            ItemNumber = product.ItemNumber,
            ProductName = product.ProductName,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Description = product.Description,
            Supplier = supplier
        };

        context.Products.Add(item);
        await context.SaveChangesAsync();

        return item.Id;
            
        }catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
