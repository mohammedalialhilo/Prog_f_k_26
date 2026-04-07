using eShop.DTOs.Customers;
using eShop.Entities;
using eShop.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomersController(IGenericRepository<Customer> repo) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult> ListAllCustomers()
    {
        var customers = await repo.ListAllAsync();
        return Ok(new { Success = true, StatusCode = 200, Items = customers.Count, Data = customers });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindCustomer(int id)
    {
        var customer = await repo.FindByIdAsync(id);
        return Ok(new { Success = true, StatusCode = 200, Items = 1, Data = customer });
    }

    [HttpPost()]
    public async Task<ActionResult> AddCustomer(Customer model)
    {
        try
        {
             repo.Add(model);
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
    [HttpGet("firstname/{name}")]
    public async Task<ActionResult> FindCustomerByFirstName(string name)
    {
        try
        {
            var customer = await repo.FindAsync(c => c.FirstName == name);
            if (customer is null) return NotFound();

            return Ok(new { Success = true, StatusCode = 200, Items = 1, Data = customer });
        }
        catch
        {
            
            return NotFound("Hittade inte leverantör");
        }
    }
    [HttpGet("lastname/{name}")]
    public async Task<ActionResult> FindCustomerByLastName(string name)
    {
        try
        {
            var customer = await repo.FindAsync(c => c.LastName == name);
            if (customer is null) return NotFound();

            return Ok(new { Success = true, StatusCode = 200, Items = 1, Data = customer });
        }
        catch
        {
            
            return NotFound("Hittade inte leverantör");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCustomer(int id, Customer model)
    {
       try
        {
            model.Id = id;
            repo.Update(model);
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
    public async Task<ActionResult> DeleteCustomer(int id)
    {
       try
        {
            var customer = await repo.FindByIdAsync(id);
            if(customer is null) return BadRequest("Hittade inte kunden");
            repo.Delete(customer);
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

