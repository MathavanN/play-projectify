
using Microsoft.EntityFrameworkCore;
using PlayProjectify.ApiService.Apis;
using PlayProjectify.ApiService.Data;
using PlayProjectify.ApiService.Services;
using PlayProjectify.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.AddApiVersioning();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=apiapp.db"));
builder.Services.AddScoped<IOrderService, OrderService>();
string[] versions = ["v1"];
builder.AddDefaultOpenApi(versions);

var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    await db.Database.MigrateAsync();
//    await DbSeeder.SeedAsync(db);
//}

app.MapDefaultEndpoints();
app.MapWeatherApi();
app.MapOrderApi();
//app.MapCategoryApi();
app.UseDefaultOpenApi(versions);
app.Run();
