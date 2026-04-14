using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MormorDagnysApi.Data;
using MormorDagnysApi.DTOs.Suppliers;
using MormorDagnysApi.Entities;

namespace MormorDagnysApi.Controllers;

[Route("api/suppliers")]
[ApiController]
public class SuppliersController(MormorDagnysContext context) : ControllerBase
{
    [HttpGet("search")]
    public async Task<ActionResult> SearchSuppliers([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Du maste ange ett leverantorsnamn.");
        }

        var searchTerm = name.Trim();

        var suppliers = await context.Suppliers
            .Include(s => s.SupplierProducts)
            .ThenInclude(sp => sp.Product)
            .Where(s => EF.Functions.Like(s.Name, $"%{searchTerm}%"))
            .OrderBy(s => s.Name)
            .ToListAsync();

        var result = suppliers.Select(MapSupplier).ToList();

        return Ok(new { Success = true, StatusCode = 200, Items = result.Count, Data = result });
    }

    [HttpPost("{supplierId}/products")]
    public async Task<ActionResult> AddProductToSupplier(int supplierId, PostSupplierProductDto supplierProduct)
    {
        var supplier = await context.Suppliers.FindAsync(supplierId);
        if (supplier is null)
        {
            return NotFound("Hittade ingen leverantor.");
        }

        var articleNumber = supplierProduct.ArticleNumber.Trim();
        var productName = supplierProduct.Name.Trim();

        var product = await context.Products
            .SingleOrDefaultAsync(p => p.ArticleNumber == articleNumber);

        if (product is null)
        {
            product = new Product
            {
                ArticleNumber = articleNumber,
                Name = productName
            };

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }
        else if (!string.Equals(product.Name, productName, StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Artikelnumret finns redan med ett annat produktnamn.");
        }

        var exists = await context.SupplierProducts
            .AnyAsync(sp => sp.SupplierId == supplierId && sp.ProductId == product.ProductId);

        if (exists)
        {
            return Conflict("Produkten finns redan hos leverantoren.");
        }

        var newSupplierProduct = new SupplierProduct
        {
            SupplierId = supplierId,
            ProductId = product.ProductId,
            PricePerKg = supplierProduct.PricePerKg
        };

        await context.SupplierProducts.AddAsync(newSupplierProduct);
        await context.SaveChangesAsync();

        return StatusCode(201, new
        {
            Success = true,
            StatusCode = 201,
            Data = new
            {
                SupplierId = supplierId,
                ProductId = product.ProductId,
                product.ArticleNumber,
                product.Name,
                newSupplierProduct.PricePerKg
            }
        });
    }

    [HttpPatch("{supplierId}/products/{productId}/price")]
    public async Task<ActionResult> UpdateProductPrice(int supplierId, int productId, PatchSupplierProductPriceDto request)
    {
        var supplierProduct = await context.SupplierProducts
            .Include(sp => sp.Product)
            .SingleOrDefaultAsync(sp => sp.SupplierId == supplierId && sp.ProductId == productId);

        if (supplierProduct is null)
        {
            return NotFound("Hittade ingen produkt hos den leverantoren.");
        }

        supplierProduct.PricePerKg = request.PricePerKg;
        await context.SaveChangesAsync();

        return Ok(new
        {
            Success = true,
            StatusCode = 200,
            Data = new
            {
                supplierProduct.SupplierId,
                supplierProduct.ProductId,
                supplierProduct.Product.ArticleNumber,
                supplierProduct.Product.Name,
                supplierProduct.PricePerKg
            }
        });
    }

    private static GetSupplierDto MapSupplier(Supplier supplier)
    {
        return new GetSupplierDto
        {
            SupplierId = supplier.SupplierId,
            Name = supplier.Name,
            Address = supplier.Address,
            ContactPerson = supplier.ContactPerson,
            PhoneNumber = supplier.PhoneNumber,
            Email = supplier.Email,
            Products = supplier.SupplierProducts
                .OrderBy(sp => sp.Product.Name)
                .Select(sp => new GetSupplierProductDto
                {
                    ProductId = sp.ProductId,
                    ArticleNumber = sp.Product.ArticleNumber,
                    Name = sp.Product.Name,
                    PricePerKg = sp.PricePerKg
                })
                .ToList()
        };
    }
}
