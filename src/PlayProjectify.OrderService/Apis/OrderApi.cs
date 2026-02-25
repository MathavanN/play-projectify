using Asp.Versioning;
using Asp.Versioning.Builder;
using PlayProjectify.OrderService.Models.DTO;
using PlayProjectify.OrderService.Services;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.OrderService.Apis;

public static class  OrderApi
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
        var api = app.MapGroup("api/v{version:apiVersion}/order")
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

        api.MapPost("/", CreateOrderV1)
            .WithName("CreateOrder")
            .WithSummary("create new order")
            .WithDescription("create new order")
            .WithTags("Order")
            .Validate<CreateOrderDto>()
            .Produces<ProjectifyServiceResult<OrderDto>>(201)
            .Produces<ProjectifyServiceResult>(400);

        api.MapPut("/{id:guid}/cancel", CancelOrderV1)
            .WithName("CancelOrder")
            .WithSummary("Cancel an order")
            .WithDescription("Cancel an order")
            .WithTags("Order")
            .Produces(204)
            .Produces<ProjectifyServiceResult>(400)
            .Produces<ProjectifyServiceResult>(404);
    }

    private static async Task<IResult> GetOrdersV1(IOrderService orderService)
    {
        var result = await orderService.GetAll();
        return result.ToApiResult();
    }

    private static async Task<IResult> GetOrderV1(IOrderService orderService, Guid id)
    {
        var result = await orderService.Get(id);
        return result.ToApiResult();
    }

    private static async Task<IResult> CreateOrderV1(IOrderService orderService, CreateOrderDto order)
    {
        var result = await orderService.Add(order);
        return result.ToApiResult("GetOrder", data => new { id = data.OrderId });
    }

    private static async Task<IResult> CancelOrderV1(IOrderService orderService, Guid id)
    {
        var result = await orderService.Cancel(id);
        return result.ToApiNoContentResult();
    }
}