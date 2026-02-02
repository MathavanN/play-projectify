using FluentValidation;
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

builder.Services.AddSingleton<ProductStore>();
builder.Services.AddScoped<IProductService, InMemoryProductService>();

string[] versions = ["v1", "v2"]; 
builder.AddDefaultOpenApi(versions);

var app = builder.Build();
app.MapDefaultEndpoints();
app.MapAboutApi();
app.MapProductApi();
app.UseDefaultOpenApi(versions);
app.Run();
