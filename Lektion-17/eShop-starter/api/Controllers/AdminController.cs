using AutoMapper;
using core.Interfaces;
using api.DTOs.Orders;
using Microsoft.AspNetCore.Mvc;
using core.Entities;

namespace api.Controllers;

public class AdminController(IUnitOfWork uow, IMapper mapper) : ApiBaseController
{
    [HttpPost("delivery")]
    public async Task<ActionResult> AddDeliveryMethod(PostDeliveryMethodDto deliveryDto)
    {
        var deliveryMethod = mapper.Map<DeliveryMethod>(deliveryDto);
        uow.Repository<DeliveryMethod>().Add(deliveryMethod);

        if(await uow.Complete()) return Ok(deliveryMethod);
        return Ok();
    }
    // [HttpGet("delivery")]
    // public async Task<ActionResult> GetAllDeliveryMethods()
    // {
    //     return Ok(await uow.Repository<DeliveryMethod>().ListAsync()); 
    // }

}
