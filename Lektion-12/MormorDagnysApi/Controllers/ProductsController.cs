using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MormorDagnysApi.Data;
using MormorDagnysApi.DTOs.Products;
using MormorDagnysApi.Entities;

namespace MormorDagnysApi.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController(MormorDagnysContext context) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult> ListAllProducts()
    {
        var products = await context.Products
            .Include(p => p.SupplierProducts)
            .ThenInclude(sp => sp.Supplier)
            .OrderBy(p => p.Name)
            .ToListAsync();

        var result = products.Select(MapProduct).ToList();

        return Ok(new { Success = true, StatusCode = 200, Items = result.Count, Data = result });
    }

    [HttpGet("search")]
    public async Task<ActionResult> SearchProducts([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Du maste ange ett produktnamn.");
        }

        var searchTerm = name.Trim();

        var products = await context.Products
            .Include(p => p.SupplierProducts)
            .ThenInclude(sp => sp.Supplier)
            .Where(p => EF.Functions.Like(p.Name, $"%{searchTerm}%"))
            .OrderBy(p => p.Name)
            .ToListAsync();

        var result = products.Select(MapProduct).ToList();

        return Ok(new { Success = true, StatusCode = 200, Items = result.Count, Data = result });
    }

    private static GetProductDto MapProduct(Product product)
    {
        return new GetProductDto
        {
            ProductId = product.ProductId,
            ArticleNumber = product.ArticleNumber,
            Name = product.Name,
            Suppliers = product.SupplierProducts
                .OrderBy(sp => sp.Supplier.Name)
                .Select(sp => new GetProductSupplierDto
                {
                    SupplierId = sp.SupplierId,
                    Name = sp.Supplier.Name,
                    PricePerKg = sp.PricePerKg
                })
                .ToList()
        };
    }
}
