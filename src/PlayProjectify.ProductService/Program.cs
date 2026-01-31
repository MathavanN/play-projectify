using PlayProjectify.ProductService.Apis;
using PlayProjectify.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.AddApiVersioning();
string[] versions = ["v1", "v2"]; 
builder.AddDefaultOpenApi(versions);

var app = builder.Build();
app.MapDefaultEndpoints();
app.MapAboutApi();
app.UseDefaultOpenApi(versions);
app.Run();
