using eShop.Data;
using eShop.DTOs.Customers;
using eShop.Entities;
using eShop.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomersController(IUnitOfWork uow) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult> ListAllCustomers()
    {
        var customers = await uow.customerRepository.ListAllCustomer();

       

        return Ok(new { Success = true, StatusCode = 200, Items = customers.Count, Data = customers });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindCustomer(int id)
    {

        var customer = await uow.customerRepository.FindCustomer(id);
       

        return Ok(new { Success = true, StatusCode = 200, Items = "Not defined", Data = customer });
    }

    // [HttpGet("{id}/cart")]
    // public async Task<ActionResult> ListCartContent(string id)
    // {
    //     // Vem är den inloggade användaren?
    //     var user = User;
    //     var email = user.Identity.Name;

    //     // Vem tillhör varukorgen?
    //     var customer = await userManager.FindByIdAsync(id);

    //     if (email != customer.Email) return BadRequest();

    //     return Ok("Det är min kundkorg");

    //     // Cart cart = await context.Carts
    //     //     .Include(c => c.Customer)
    //     //     .Include(c => c.CartItems)
    //     //     .ThenInclude(c => c.Product)
    //     //     .SingleOrDefaultAsync(c => c.CustomerId == id);

    //     // var data = new
    //     // {
    //     //     CreatedDate = cart.CreatedDate.ToShortDateString(),
    //     //     cart.Customer.FirstName,
    //     //     cart.Customer.LastName,
    //     //     cart.Customer.Email,
    //     //     Total = cart.CartItems.Sum(p => p.Price * p.Quantity),
    //     //     Items = cart.CartItems.Select(c => new
    //     //     {
    //     //         c.ProductId,
    //     //         c.Product.ProductName,
    //     //         c.Price,
    //     //         c.Quantity,
    //     //         LineSum = c.Price * c.Quantity
    //     //     })
    //     // };

    //     // return Ok(new
    //     // {
    //     //     Success = true,
    //     //     StatusCode = 200,
    //     //     Items = cart.CartItems.Count,
    //     //     Data = data
    //     // });



    // }

    [HttpPost()]
    public async Task<ActionResult> AddCustomer(PostCustomerDto model)
    {
        try
        {
          if( await uow.customerRepository.AddCustomer(model)){
            await uow.Complete();
            return StatusCode(201, model);
            }
            
            return StatusCode(500, "Något server fel inträffade");
            
        }
        catch 
        {
            return StatusCode(500, "Något server fel inträffade");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCustomer(int id, PutCustomerDto model)
    {try
        {
          if( await uow.customerRepository.UppdateCustomer(id,model)){
            await uow.Complete();
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
          if(await uow.customerRepository.DeleteCustomer(id)){ 
            await uow.Complete();
            return NoContent();
            }

        return StatusCode(500, "Något server fel inträffade");
    }
    catch
    {
        
         return StatusCode(500, "Något server fel inträffade");
    }
    }
}

