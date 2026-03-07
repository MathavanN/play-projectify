using Asp.Versioning;
using Asp.Versioning.Builder;
using PlayProjectify.ProductService.Models.DTO;
using PlayProjectify.ProductService.Services;
using PlayProjectify.ServiceDefaults;

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
            .Produces<ProjectifyServiceResult<IEnumerable<GetProductDto>>>(200);

        api.MapPost("/lookupByIds", GetProductByIdsV1)
            .WithName("LookupProductByIds")
            .WithSummary("Get products by Ids")
            .WithDescription("Get products by Ids")
            .WithTags("Product")
            .Produces<ProjectifyServiceResult<IEnumerable<GetProductDto>>>(200);

        api.MapGet("/{id:guid}", GetProductV1)
            .WithName("GetProduct")
            .WithSummary("Get a product")
            .WithDescription("Get a product")
            .WithTags("Product")
            .Produces<ProjectifyServiceResult<GetProductDto>>(200)
            .Produces<ProjectifyServiceResult>(404);

        api.MapPost("/", AddProductV1)
            .WithName("AddProduct")
            .WithSummary("Add product")
            .WithDescription("Add new product")
            .WithTags("Product")
            .Validate<AddProductDto>()
            .Produces<ProjectifyServiceResult<ProductDto>>(201)
            .Produces<ProjectifyServiceResult>(400);

        api.MapPut("/reserve", ReserveProductsV1)
            .WithName("ReserveProducts")
            .WithSummary("Reserve products")
            .WithDescription("Reserve products")
            .WithTags("Product")
            .Validate<ProductInventoryDto>()
            .Produces(204)
            .Produces<ProjectifyServiceResult>(400)
            .Produces<ProjectifyServiceResult>(404);

        api.MapPut("/release", ReleaseProductsV1)
            .WithName("ReleaseProducts")
            .WithSummary("Release products")
            .WithDescription("Release products")
            .WithTags("Product")
            .Validate<ProductInventoryDto>()
            .Produces(204)
            .Produces<ProjectifyServiceResult>(400)
            .Produces<ProjectifyServiceResult>(404);
    }

    private static async Task<IResult> GetProductsV1(IProductService productService, CancellationToken cancellationToken)
    {
        var result = await productService.GetAll(cancellationToken);
        return result.ToApiResult();
    }

    private static async Task<IResult> GetProductV1(IProductService productService, Guid id, CancellationToken cancellationToken)
    {
        var result = await productService.Get(id, cancellationToken);
        return result.ToApiResult();
    }

    private static async Task<IResult> GetProductByIdsV1(IProductService productService, LookupIdsDto lookup, CancellationToken cancellationToken)
    {
        var result = await productService.GetByIds(lookup.ProductIds, cancellationToken);
        return result.ToApiResult();
    }

    private static async Task<IResult> AddProductV1(IProductService productService, AddProductDto product, CancellationToken cancellationToken)
    {
        var result = await productService.Add(product, cancellationToken);
        return result.ToApiResult("GetProduct", data => new { id = data.ProductId });
    }

    private static async Task<IResult> ReserveProductsV1(IProductService productService, ProductInventoryDto product, CancellationToken cancellationToken)
    {
        var result = await productService.ReserveStock(product, cancellationToken);
        return result.ToApiResult();
    }

    private static async Task<IResult> ReleaseProductsV1(IProductService productService, ProductInventoryDto product, CancellationToken cancellationToken)
    {
        var result = await productService.ReleaseStock(product, cancellationToken);
        return result.ToApiResult();
    }
}