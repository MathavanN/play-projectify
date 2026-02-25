using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlayProjectify.OrderService.ApiClients;
using PlayProjectify.OrderService.Apis;
using PlayProjectify.OrderService.Data;
using PlayProjectify.OrderService.Services;
using PlayProjectify.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.AddApiVersioning();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlite("Data Source=order.db"));
builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>(client =>
{
    client.BaseAddress = new("https+http://productservice"); ;
});
builder.Services.AddScoped<IOrderService, OrderService>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();

string[] versions = ["v1", "v2"];
builder.AddDefaultOpenApi(versions);

var app = builder.Build();
app.MapDefaultEndpoints();
app.MapAboutApi();
app.MapOrderApi();
//app.MapCategoryApi();
app.UseDefaultOpenApi(versions);
app.Run();
