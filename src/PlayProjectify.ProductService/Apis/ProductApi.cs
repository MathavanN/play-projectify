using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using PlayProjectify.ProductService.Models.DTO;
using PlayProjectify.ProductService.Services;
using PlayProjectify.ServiceDefaults;
using System.ComponentModel;

namespace PlayProjectify.ProductService.Apis;

public static class ProductApi
{
    public static IEndpointRouteBuilder MapProductApi(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewVersionedApi("Product")
            .NewApiVersionSet()
                    .HasApiVersion(new ApiVersion(1.0))
                    .ReportApiVersions()
                    .Build();

        app.MapProductApiV1(versionSet);

        return app;
    }

    private static void MapProductApiV1(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var api = app.MapGroup("api/v{version:apiVersion}/product")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1, 0);

        api.MapGet("/", GetProductsV1)
            .WithName("GetProducts")
            .WithSummary("Get products")
            .WithDescription("Get all products")
            .WithTags("Product")
            .Produces<ProjectifyServiceResult<IEnumerable<ProductDto>>>(200);

        api.MapGet("/{id:guid}", GetProductV1)
            .WithName("GetProduct")
            .WithSummary("Get a product")
            .WithDescription("Get a product")
            .WithTags("Product")
            .Produces<ProjectifyServiceResult<ProductDto>>(200)
            .Produces<ProjectifyServiceResult>(404);

        api.MapPost("/", AddProductV1)
            .WithName("AddProduct")
            .WithSummary("Add product")
            .WithDescription("Add new product")
            .WithTags("Product")
            .Validate<AddProductDto>()
            .Produces<ProjectifyServiceResult<ProductDto>>(201)
            .Produces<ProjectifyServiceResult>(400);
    }

    private static IResult GetProductsV1(IProductService productService)
    {
        return productService.GetAll().ToApiResult();
    }

    private static IResult GetProductV1(IProductService productService, [Description("The product id")] Guid id)
    {
        return productService.Get(id).ToApiResult();
    }

    private static IResult AddProductV1(IProductService productService, AddProductDto product)
    {
        return productService.Add(product).ToApiResult("GetProduct", data => new { id = data.ProductId });
    }
}