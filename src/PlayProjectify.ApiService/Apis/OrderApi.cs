using Asp.Versioning;
using Asp.Versioning.Builder;
using PlayProjectify.ApiService.Models.DTO;
using PlayProjectify.ApiService.Services;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ApiService.Apis;

public static class OrderApi
{
    public static IEndpointRouteBuilder MapOrderApi(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewVersionedApi("Order")
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        app.MapOrderApiV1(versionSet);

        return app;
    }

    private static void MapOrderApiV1(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var api = app.MapGroup("api/v{version:apiVersion}/Order")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1, 0);


        api.MapGet("/", GetOrdersV1)
            .WithName("GetOrders")
            .WithSummary("Get orders")
            .WithDescription("Get all orders")
            .WithTags("Order")
            .Produces<ProjectifyServiceResult<IEnumerable<OrderDto>>>(200);

        api.MapGet("/{id:guid}", GetOrderV1)
            .WithName("GetOrder")
            .WithSummary("Get an order")
            .WithDescription("Get an order")
            .WithTags("Order")
            .Produces<ProjectifyServiceResult<OrderDto>>(200)
            .Produces<ProjectifyServiceResult>(404);


    }
    private static async Task<IResult> GetOrdersV1(IOrderService orderService, CancellationToken cancellationToken)
    {
        var result = await orderService.GetAll(cancellationToken);
        return result.ToApiResult();
    }
    private static async Task<IResult> GetOrderV1(IOrderService orderService, Guid id, CancellationToken cancellationToken)
    {
        var result = await orderService.Get(id, cancellationToken);
        return result.ToApiResult();
    }
}