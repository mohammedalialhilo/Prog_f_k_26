using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using simple_api.Data;
using simple_api.Models;

namespace simple_api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly string _path;

    public ProductsController(IWebHostEnvironment environment)
    {
        _environment = environment;
        _path = string.Concat(_environment.ContentRootPath, "/Data/products.json");


    }
    [HttpGet()]
    public ActionResult ListProducts()
    {

        var products = Storage<Product>.ReadJson(_path);
        return Ok(new { status = "Success", StatusCode = "200", items = products.Count, Data = products });
    }
    [HttpGet("{id}")]

    public ActionResult FindProduct(int id)
    {
        var products = Storage<Product>.ReadJson(_path);

        Product product = products.SingleOrDefault(c => c.Id == id);
        if (product == null) return NotFound(new { Success = false, Message = "Hittar inte produkt" });

        return Ok(new { status = "Success", StatusCode = "200", Data = product });
    }
    [HttpGet("/search/{Product}")]

    public ActionResult FindProductByName(string Product)
    {
        var products = Storage<Product>.ReadJson(_path);

        Product product = products.SingleOrDefault(c => c.ProductName == Product);
        if (product == null) return NotFound("Inget produkt fins....");

        return Ok(new { status = "Success", StatusCode = "200", Data = product });
    }

    [HttpPost()]
    public ActionResult AddProduct(Product product)
    {
        var products = Storage<Product>.ReadJson(_path);
        products.Add(product);
        Storage<Product>.WriteJson(_path, products);


        return CreatedAtAction(nameof(FindProduct), new { id = product.Id }, product);
    }
    [HttpPut("{id}")]
    public ActionResult UpdateProduct(int id, Product product)
    {
        var products = Storage<Product>.ReadJson(_path);
        Product item = products.SingleOrDefault(c => c.Id == id);
        if (product == null) return NotFound("Inget produkt fins....");

        products.Remove(item);
        products.Add(product);
        Storage<Product>.WriteJson(_path, products);
        return CreatedAtAction(nameof(FindProduct), new { id = product.Id }, product);
    }
    [HttpDelete("{id}")]
    public ActionResult DeleteProduct(int id)
    {
        var products = Storage<Product>.ReadJson(_path);
        Product product = products.FirstOrDefault(c => c.Id == id);
        products.Remove(product);
        Storage<Product>.WriteJson(_path, products);

        return Ok(products);
    }
}

