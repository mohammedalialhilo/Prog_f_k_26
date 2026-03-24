using eShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using eShop.Entities;
using System.Text;
using eShop.Services;
using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using MySql.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EShopContext>(options =>
{
    // options.UseSqlite(
    //     builder.Configuration.GetConnectionString("sqlitedev"));
    // options.UseMySql(builder.Configuration.GetConnectionString("mysqldev"));
    options.UseMySql(builder.Configuration.GetConnectionString("mysqldev"));
});

builder.Services.AddIdentityCore<User>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EShopContext>();


builder.Services.AddScoped<TokenService>();


builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("tokenSettings:tokenKey").Value))
    };
});

builder.Services.AddAuthorizationBuilder().AddPolicy("RequireCorprateRights", policy => policy.RequireRole("Admin", "Manager"));
builder.Services.AddAuthorizationBuilder().AddPolicy("RequireAdminRights", policy => policy.RequireRole("Admin"));
builder.Services.AddAuthorizationBuilder().AddPolicy("RequireSalesRights", policy => policy.RequireRole("Admin", "Manager", "Sales"));

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
