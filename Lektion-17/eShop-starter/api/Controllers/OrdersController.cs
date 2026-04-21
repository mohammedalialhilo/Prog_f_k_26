using api.DTOs.Orders;
using api.Extensions;
using core.Entities;
using core.Entities.Orders;
using core.Interfaces;
using core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;
[Authorize]
public class OrdersController(ICartService cartService, IUnitOfWork uow):ApiBaseController
{
    [HttpPost()]
    public async Task<ActionResult> CreateOrder(PostOrderDto orderDto)
    {
        var email = User.GetUserEmail();
        var cart = await cartService.GetCartAsync(orderDto.CartId);
        if(cart is null) return BadRequest($"ingen kundvagn hittades för användare {email}");

        var items = new List<OrderItem>();
        foreach (var item in cart.Items)
        {
            var product = await uow.Repository<Product>().FindByIdAsync(item.ProductId);
            if(product is null)return BadRequest("problem med produkten");
            
            var productItem = new ItemOrdered
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
            };

            var orderItem = new OrderItem
            {
              ItemOrdered = productItem,
              Price = product.Price,
              Quantity = item.Quantity  
            };

            items.Add(orderItem);
            
        }

        var deliveryMethod = await uow.Repository<DeliveryMethod>().FindByIdAsync(orderDto.DeliveryMethodId);
        if(deliveryMethod is null) return BadRequest("Leverans sätt saknas eller felaktigt");


        var order = new Order
        {
            OrderItems = items,
            DeliveryMethod = deliveryMethod,
            ShippingAddress = orderDto.ShippingAddress,
            PaymentInfo = orderDto.PaymentInfo,
            CustomerEmail = email,
            SubTotal = items.Sum(c => c.Price * c.Quantity)
        };

        uow.Repository<Order>().Add(order);
        if(await uow.Complete()) return StatusCode(201, order);

        return BadRequest("Det gick inte att slutföra beställningen");
    }
    [HttpGet()]
    public async Task<ActionResult> GetOrdersForUser()
    {
        var spec = new OrderSpecification(User.GetUserEmail());
        var orders = await uow.Repository<Order>().ListAsync(spec);

        var result = orders.Select(c => c.ToDTO()).ToList();
        return Ok(result);
    }
    [HttpGet("id")]
    public async Task<ActionResult> GetOrdersForUserById(string id)
    {
        var spec = new OrderSpecification(User.GetUserEmail(), id);
        var order = await uow.Repository<Order>().FindAsync(spec);
        if(order is null )return NotFound("hittar inte");

        return Ok(order);
    }
}
