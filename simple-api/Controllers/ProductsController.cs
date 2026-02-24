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
        return Ok(products);
    }
    [HttpGet("search/{productName}")]

    public ActionResult FindProduct(string productName)
    {
        var products = Storage<Product>.ReadJson(_path);

        Product product = products.FirstOrDefault(c => c.ProductName == productName);
        if (product == null) return NotFound("Inget produkt fins....");

        return Ok(product);
    }

    [HttpPost()]
    public ActionResult AddProduct(Product product)
    {
        // var products = new List<Product>
        // {
        //     (Product)Storage<Product>.ReadJson(_path),
        //     // products.Add(product);
        //     product
        // };
        // Storage<Product>.WriteJson(_path, products);
        // var save = Storage<Product>.ReadJson(_path);
        var products = Storage<Product>.ReadJson(_path);
        products.Add(product);
        Storage<Product>.WriteJson(_path, products);


        return StatusCode(201);
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
        return Ok(products);
    }
    [HttpDelete("{id}")]
    public ActionResult DeleteProduct(int id)
    {
        var products = Storage<Product>.ReadJson(_path);
        Product product = products.FirstOrDefault(c => c.Id == id);
        products.Remove(product);
        Storage<Product>.WriteJson(_path, products);

        return Ok(201);
    }
}

