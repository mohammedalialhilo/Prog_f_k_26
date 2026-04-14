using api.Helpers;
using core.Interfaces;
using infrastructure.Data;
using infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EShopContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("sqlite"));
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddAutoMapper(options =>
{
    options.LicenseKey="eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODA3MDU2MDAwIiwiaWF0IjoiMTc3NTU2NDM0OSIsImFjY291bnRfaWQiOiIwMTlkNjdlMGZjZDQ3ZTY5OTU5NGRjYjZjMDRmMWM4NiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa25reTNxc2JjZm5tdHJmejA1ejdrZ2VmIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.DtUQW0f2Dj_CtZtIeJzA6B9km7gFEgfoFAGmH-VGjUIOCQDBslyJSCEYUQ8f48oyuPZb1Odmfxi53KKqhouEUB7mQMoV4GBI4lf3Kx3vjw2QiVMGjVZNU8tTjRD-wYVRcuZ-DkzOe--iSi-e48wKIjvuwT_ntVbtHSkVRAWkuPuFaoY61YSk7p0S6FaCJb30Wp1_BqK15oeTh9zJGFKfMJU6qENDBeZUtPWmKrE-USD_jTylwUmYPY_lE2VWdgbmCKQeh2Xh6tvhzmVpL_qeTwY305XnuJ2aUzIstpIMzJqKCwFskEh00Owq7D3OTrCoQ4FC_4EqBGDRZxV8JFGeiA";
    options.AddProfile(new MappingProfiles());
});



builder.Services.AddControllers();

var app = builder.Build();


// Configure the HTTP request pipeline.

app.MapControllers();
try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EShopContext>();
    await context.Database.MigrateAsync();
    await SeedDatabase.SeedProducts(context);
}
catch (Exception ex)
{
    
    Console.WriteLine(ex.Message);
}

app.Run();
