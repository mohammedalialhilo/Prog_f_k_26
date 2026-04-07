using eShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using eShop.Entities;
using System.Text;
using eShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using eShop.Interfaces;
using eShop.Repositories;
using eShop;
using eShop.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EShopContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("sqlite"));
    // options.UseNpgsql(builder.Configuration.GetConnectionString("postgres"));
});

builder.Services.AddAutoMapper(options =>
{
    options.LicenseKey="eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODA3MDU2MDAwIiwiaWF0IjoiMTc3NTU2NDM0OSIsImFjY291bnRfaWQiOiIwMTlkNjdlMGZjZDQ3ZTY5OTU5NGRjYjZjMDRmMWM4NiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa25reTNxc2JjZm5tdHJmejA1ejdrZ2VmIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.DtUQW0f2Dj_CtZtIeJzA6B9km7gFEgfoFAGmH-VGjUIOCQDBslyJSCEYUQ8f48oyuPZb1Odmfxi53KKqhouEUB7mQMoV4GBI4lf3Kx3vjw2QiVMGjVZNU8tTjRD-wYVRcuZ-DkzOe--iSi-e48wKIjvuwT_ntVbtHSkVRAWkuPuFaoY61YSk7p0S6FaCJb30Wp1_BqK15oeTh9zJGFKfMJU6qENDBeZUtPWmKrE-USD_jTylwUmYPY_lE2VWdgbmCKQeh2Xh6tvhzmVpL_qeTwY305XnuJ2aUzIstpIMzJqKCwFskEh00Owq7D3OTrCoQ4FC_4EqBGDRZxV8JFGeiA";
    options.AddProfile(new MappingProfiles());
});
// 1. Lägg till inloggningsinställningar (authentication)...
builder.Services.AddIdentityCore<User>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EShopContext>();

// Depency Injection...
// Registera vår TokenServer i dotnet's dependency lista...
builder.Services.AddScoped<TokenService>();
// Registrera övriga tjänster...
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
// builder.Services.AddControllers(options =>
// {
//     // Skapa en generell regel(policy) som tvingar alla att vara inloggade...
//     var policy = new AuthorizationPolicyBuilder()
//         .RequireAuthenticatedUser()
//         .Build();

//     // Applicera regeln...
//     options.Filters.Add(new AuthorizeFilter(policy));

// });

// 2. Aktivera ett auktoriserings schema, dvs hur ska vi kontrollera användaren...
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
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

// 6. Ett alternativt sätt att koppla behörighet i dotnet...
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireCorporateRights", policy => policy.RequireRole("Admin", "Manager"));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminRights", policy => policy.RequireRole("Admin"));

// 3. Aktivera behörighetskontroll...
builder.Services.AddAuthorization();

var app = builder.Build();

// 4. Använd användar inloggning i systemet...
app.UseAuthentication();

// 5. Använda behörighetskontroll...
app.UseAuthorization();

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    var seed = new SeedDatabase(userManager, roleManager);
    await seed.InitDb(app);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

app.Run();
