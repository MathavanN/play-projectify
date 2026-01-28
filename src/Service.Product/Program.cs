using Microsoft.AspNetCore.Mvc;
using PlayProjectify.Core.ApiProblemBuilder;
using PlayProjectify.Core.ApiResult;
using Service.Product.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IApiProblemBuilder, ApiProblemBuilder>();
builder.Services.AddProblemDetails();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        var detail = string.Join(", ", errors.SelectMany(kvp => kvp.Value.Select(msg => $"{kvp.Key}: {msg}")));

        var problemDetails = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "Validation error",
            Status = StatusCodes.Status400BadRequest,
            Instance = context.HttpContext.Request.Path,
            Detail = detail
        };

        return new BadRequestObjectResult(ApiResult.Failure(problemDetails))
        {
            ContentTypes = { "application/problem+json" }
        };

    };
});
builder.Services.AddSingleton<ProductStore>();
builder.Services.AddScoped<IProductRepository, InMemoryProductRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();