using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlayProjectify.ProductService.Apis;
using PlayProjectify.ProductService.Data;
using PlayProjectify.ProductService.Services;
using PlayProjectify.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.AddApiVersioning();

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlite("Data Source=product.db"));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

string[] versions = ["v1", "v2"];
builder.AddDefaultOpenApi(versions);

var app = builder.Build();
app.MapDefaultEndpoints();
app.MapAboutApi();
app.MapProductApi();
app.MapCategoryApi();
app.UseDefaultOpenApi(versions);
app.Run();
