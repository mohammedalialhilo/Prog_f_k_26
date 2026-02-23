using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using simple_api.Models;

namespace simple_api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IList<Product> _products;

    public ProductsController()
    {
        _products =
        [
            new Product { Id = 1, ProductName = "Laptop", Price = 10000M },
            new Product { Id = 2, ProductName = "PC", Price = 30000M },
        ];

    }
    [HttpGet()]
    public ActionResult ListProducts()
    {



        return Ok(_products);
    }
    [HttpGet("search/{productName}")]

    public ActionResult FindProduct(string productName)
    {
        Product product = _products.FirstOrDefault(c => c.ProductName == productName);
        if (product == null) return NotFound("Inget produkt fins....");

        return Ok(product);
    }

    [HttpPost()]
    public ActionResult AddProduct(Product product)
    {
        _products.Add(product);

        return StatusCode(201, _products);
    }
    [HttpPut("{id}")]
    public ActionResult UpdateProduct(int id, Product product)
    {
        Product item = _products.FirstOrDefault(c => c.Id == id);
        if (product == null) return NotFound("Inget produkt fins....");

        _products.Remove(item);
        _products.Add(product);
        return Ok(_products);
    }
    [HttpDelete("{id}")]
    public ActionResult DeleteProduct(int id)
    {
        Product product = _products.FirstOrDefault(c => c.Id == id);
        _products.Remove(product);
        return Ok(_products);
    }
}

