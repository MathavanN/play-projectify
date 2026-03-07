using Microsoft.EntityFrameworkCore;
using PlayProjectify.OrderService.ApiClients;
using PlayProjectify.OrderService.Data;
using PlayProjectify.OrderService.Models.DTO;
using PlayProjectify.OrderService.Models.Entites;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.OrderService.Services;

public interface IOrderService
{
    Task<ProjectifyServiceResult<IEnumerable<OrderDto>>> GetAll(CancellationToken cancellationToken);
    Task<ProjectifyServiceResult<OrderDto>> Get(Guid id, CancellationToken cancellationToken);
    Task<ProjectifyServiceResult<OrderDto>> Add(CreateOrderDto newOrder, CancellationToken cancellationToken);
    Task<ProjectifyServiceResult> Cancel(Guid id, CancellationToken cancellationToken);
}

public sealed class OrderService : IOrderService
{
    private readonly OrderDbContext _dbContext;
    private readonly IProductApiClient _productApiClient;
    public OrderService(OrderDbContext dbContext, IProductApiClient productApiClient)
    {
        _dbContext = dbContext;
        _productApiClient = productApiClient;
    }

    public async Task<ProjectifyServiceResult<IEnumerable<OrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        return await _dbContext.Orders
            .Include(d => d.Items)
            .Select(o => new OrderDto(
                o.Id,
                o.CustomerId,
                o.OrderStatus.ToString(),
                o.TotalAmount,
                o.CreatedAt,
                o.Items.Select(d => new OrderItemDto(d.ProductId, d.ProductName, d.UnitPrice, d.Quantity))
                )
            )
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectifyServiceResult<OrderDto>> Get(Guid id, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .Include(d => d.Items)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (order is not null)
            return new OrderDto(order.Id, order.CustomerId, order.OrderStatus.ToString(), order.TotalAmount, order.CreatedAt, order.Items.Select(d => new OrderItemDto(d.ProductId, d.ProductName, d.UnitPrice, d.Quantity)));

        return ProjectifyServiceResult<OrderDto>.NotFound($"Order with ID {id} not found.");
    }

    public async Task<ProjectifyServiceResult<OrderDto>> Add(CreateOrderDto newOrder, CancellationToken cancellationToken)
    {
        var toInsert = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = newOrder.CustomerId,
            OrderStatus = OrderStatus.Pending,
        };

        var lookup = await _productApiClient.GetProductLookup(newOrder.Items.Select(i => i.ProductId), cancellationToken);
        if (lookup.NotFoundProductIds.Any())
            return ProjectifyServiceResult<OrderDto>.BadRequest($"Product with IDs ({string.Join(", ", lookup.NotFoundProductIds)}) not found.");

        var products = lookup.FoundProducts;
        foreach (var item in newOrder.Items)
        {
            var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);

            if (product == null)
                return ProjectifyServiceResult<OrderDto>.BadRequest($"Product with ID {item.ProductId} not found");

            if (product.ProductQuantity < item.Quantity)
                return ProjectifyServiceResult<OrderDto>.BadRequest($"Product with ID {item.ProductId} has insufficient stock");
        }

        //reverse Stock
        var items = newOrder.Items.Select(d => new ProductStockRequest(d.ProductId, d.Quantity));
        var reserve = await _productApiClient.ReserveInventoryStock(items, cancellationToken);
        if (!reserve.IsSuccess)
            return ProjectifyServiceResult<OrderDto>.BadRequest("Stock reservation failed");

        toInsert.Items = [.. newOrder.Items.Select(d =>
        {
            var product = products.First(p => p.ProductId == d.ProductId);
            return new OrderItem
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.ProductPrice,
                Quantity = d.Quantity
            };
        })];

        toInsert.TotalAmount = toInsert.Items.Sum(x => x.TotalPrice);
        _dbContext.Orders.Add(toInsert);
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            var release = await _productApiClient.ReleaseInventoryStock(items, cancellationToken);
            return ProjectifyServiceResult<OrderDto>.CommonError("Failed to place the new order.");
        }
        return new OrderDto(toInsert.Id, toInsert.CustomerId, toInsert.OrderStatus.ToString(), toInsert.TotalAmount, toInsert.CreatedAt, toInsert.Items.Select(d => new OrderItemDto(d.ProductId, d.ProductName, d.UnitPrice, d.Quantity)));
    }

    public async Task<ProjectifyServiceResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (existing is null)
            return ProjectifyServiceResult<object>.NotFound("Order not found.");

        if (existing.OrderStatus != OrderStatus.Pending)
            return ProjectifyServiceResult<object>.BadRequest("Only pending orders can be cancelled.");

        var release = await _productApiClient.ReleaseInventoryStock(existing.Items.Select(d => new ProductStockRequest(d.ProductId, d.Quantity)), cancellationToken);
        if (!release.IsSuccess)
            return ProjectifyServiceResult<object>.CommonError("Failed to cancel the order.");

        existing.OrderStatus = OrderStatus.Cancelled;
        existing.UpdatedAt = DateTime.UtcNow;
        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        if (result == 0)
            return ProjectifyServiceResult<object>.CommonError("Failed to cancel the order.");
        else return ProjectifyServiceResult.Success();
    }
}
