using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eShop.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace eShop.Services;

public class TokenService(UserManager<User> userManager, IConfiguration config)
{
    public async Task<string> CreateToken(User user)
    {
        List<Claim> claims = [
            new(ClaimTypes.Email,user.Email),
            new(ClaimTypes.Name,user.UserName),
            new("FirstName",user.FirstName),
            new("LastName",user.LastName)
        ];

        var roles = await userManager.GetRolesAsync(user);
        foreach(var role in roles)
        {
            claims.Add(new(ClaimTypes.Role,role));
        }

        //skapar signatur
        var tokenKey = config["tokenSettings:tokenKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var credantials = new SigningCredentials(key,SecurityAlgorithms.HmacSha512);

        //skapar biljetten
        var options = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims:claims,
            expires: DateTime.Now.AddDays(5),
            signingCredentials: credantials
        );



        return new JwtSecurityTokenHandler().WriteToken(options);
    }
}
