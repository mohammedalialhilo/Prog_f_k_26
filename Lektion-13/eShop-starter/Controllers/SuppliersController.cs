using AutoMapper;
using eShop.DTOs.Suppliers;
using eShop.Entities;
using eShop.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Controllers;

[Route("api/suppliers")]
[ApiController]
public class SuppliersController(IGenericRepository<Supplier> repo,IMapper mapper) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult> ListAllSuppliers()
    {
        try
        {
            var suppliers = await repo.ListAllAsync();;
            return Ok(new { Success = true, StatusCode = 200, Items = suppliers.Count, Data = suppliers });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Sorry, något gick fel {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindSupplier(int id)
    {
        try
        {
            var supplier = await repo.FindByIdAsync(id);
            return Ok(new { Success = true, StatusCode = 200, Items = 1, Data = supplier });
        }
        catch
        {
            return NotFound("Hittade inget");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult> FindSupplierByName(string name)
    {
        try
        {
            var supplier = await repo.FindAsync(c => c.SupplierName == name);
            if (supplier is null) return NotFound();

            return Ok(new { Success = true, StatusCode = 200, Items = 1, Data = supplier });
        }
        catch
        {
            
            return NotFound("Hittade inte leverantör");
        }
    }

    [HttpPost()]
    public async Task<ActionResult> AddSupplier(PostSupplierDto model)
    {
        try
        {
            var supplier = mapper.Map<Supplier>(model);
             repo.Add(supplier);
            if( await repo.SaveAllAsync())
            {
                return StatusCode(201);
            }

            return StatusCode(500, "Något gick när vi skulle spara ny leverantör");
        }
        catch
        {
            return StatusCode(500, "Något gick när vi skulle spara ny leverantör");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSupplier(int id, Supplier supplier)
    {
        try
        {
            supplier.Id = id;
            repo.Update(supplier);
            if( await repo.SaveAllAsync())
            {
                return NoContent();
            }
            return StatusCode(500, "Något server fel inträffade");
        }
        catch
        {
            return StatusCode(500, "Något server fel inträffade");
        }

    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveSupplier(int id)
    {
        try
        {
            var supplier = await repo.FindByIdAsync(id);
            if(supplier is null) return BadRequest("Hittade inte supplier");
            repo.Delete(supplier);
            if( await repo.SaveAllAsync())
            {
                return StatusCode(201);
            }
            return StatusCode(500, "Ett server fel inträffade");
        }
        catch
        {
            return StatusCode(500, "Ett server fel inträffade");
        }

    }
}

