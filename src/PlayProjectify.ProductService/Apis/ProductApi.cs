using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using PlayProjectify.ProductService.Models.DTOs;
using PlayProjectify.ProductService.Services;
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
            .WithTags("Product");

        api.MapGet("/{id:guid}", GetProductV1)
            .WithName("GetProduct")
            .WithSummary("Get a product")
            .WithDescription("Get a product")
            .WithTags("Product");
    }

    private static async Task<Ok<IEnumerable<ProductDto>>> GetProductsV1(IProductService productService)
    {
        return TypedResults.Ok(productService.GetAll());
    }

    private static async Task<Ok<ProductDto>> GetProductV1(IProductService productService, [Description("The product id")] Guid id)
    {
        return TypedResults.Ok(productService.Get(id));
    }
}