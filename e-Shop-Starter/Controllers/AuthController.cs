using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using eShop.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TextTemplating;
using eShop.Entities;

namespace eShop.Controllers;

    [Authorize()]
    [Route("api/auth")]
    [ApiController]
    public class AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(PostRegisterUser model)
    {
        try
        {
            
        var user = new User{
            Email = model.Email,
            UserName = model.Email,
            PhoneNumber = model.Phone,
            FirstName = model.FirstName,
            LastName = model.LastName
            
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return StatusCode(500, result.Errors);
        }

        await userManager.AddToRoleAsync(user, "User");
        return StatusCode(201);
        }catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [AllowAnonymous]
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
    [AllowAnonymous]
    [HttpPost("role")]
    public async Task<ActionResult> CreateRole([FromQuery]string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);

        if(role is null)
        {
            role = new IdentityRole{Name = roleName};
            await roleManager.CreateAsync(role);
        }
        return Ok();
    }
    [AllowAnonymous]
    [HttpPost("userrole")]
    public async Task<ActionResult> AddUserToRole([FromQuery] string userName, string roleName)
    {
        var user = await userManager.FindByNameAsync(userName);
        var role = await roleManager.FindByNameAsync(roleName);
        if(user is null && role is null) return BadRequest("Användare eller role fins inte...");

        await userManager.AddToRoleAsync(user, roleName);
        return Ok();
       
    }
}

