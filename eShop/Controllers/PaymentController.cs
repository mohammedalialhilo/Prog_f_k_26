using eShop.Data;
using eShop.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.Controllers;

[Route("api/payments")]
[ApiController]
public class PaymentController(EShopContext context) : ControllerBase
{
    [HttpPost()]
    public async Task<IActionResult> Pay([FromQuery] int customerId)
    {
        Customer customer = await context.Customers.FindAsync(customerId);
        if (customer == null) return BadRequest("Customer not found");

        Cart cart = await context.Carts.Include(c => c.CartItems).SingleOrDefaultAsync(c => c.CustomerId == customerId);
        if (cart == null) return BadRequest("Cart is empty");

        List<OrderItem> items = [.. cart.CartItems.Select(o => new OrderItem
        {
            ProductId = o.ProductId,
            Quantity = o.Quantity,
            Price = o.Price
        })];

        SalesOrder order = new()
        {
            CustomerId = customer.CustomerId,
            OrderItems = items
        };
        context.SalesOrders.Add(order);
        context.Carts.Remove(cart);

        await context.SaveChangesAsync();
        return StatusCode(201);

    }
}

