using eShop.DTOs.Suppliers;
using eShop.Entities;
using eShop.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZstdSharp.Unsafe;

namespace eShop.Controllers;

[Route("api/suppliers")]
[ApiController]
public class SuppliersController(ISupplierReopsitory repo) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult> ListAllSuppliers()
    { try{
        List<GetSuppliersDto> suppliers = await repo.ListAllSuppliers();
        return Ok(new { Success = true, StatusCode = 200, Items = suppliers.Count, Data = suppliers });
    }
        catch
        {
            return StatusCode(500 , "Sorry, Något gick fel");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindSupplier(int id)
    {
        try
        {
            GetSupplierDto supplier = await repo.FindSupplier(id);
         return Ok(new { Success = true, StatusCode = 200, Items = 1, Data = supplier });
        }
        catch 
        {
            return NotFound();
        }
    }

    [HttpPost()]
    public async Task<ActionResult> AddSupplier(PostSupplierDto supplier)
    {
        try
        {
            var id = await repo.AddSupplier(supplier);

        if(id > 0){
             return CreatedAtAction(nameof(FindSupplier), new { id }, supplier);
             }
        else
        {
            return BadRequest("Vi saknar info");
        }
        }
        catch
        {
            return StatusCode(500, "Något gick fel vid sparningen");
            
        }
    }
}

