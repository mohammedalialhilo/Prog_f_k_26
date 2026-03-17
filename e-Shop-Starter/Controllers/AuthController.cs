using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using eShop.DTOs.Auth;

namespace eShop.Controllers;

    [Route("api/auth")]
    [ApiController]
    public class AuthController(UserManager<IdentityUser> userManager) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(PostRegisterUser model)
    {
        try
        {
            
        var user = new IdentityUser{
            Email = model.Email,
            UserName = model.Email,
            PhoneNumber = model.Phone
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return StatusCode(500, result.Errors);
        }
        return StatusCode(201);
        }catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpPost("login")]
    public async Task<ActionResult> LoginUser(PostLoginUser model)
    {
        var user = await userManager.FindByNameAsync(model.Email);

        if(user is null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized(new{Success=false, message="Du är inte behörig"});
        }
        return Ok(new{Success = true , Email= user.Email});
    }
}

