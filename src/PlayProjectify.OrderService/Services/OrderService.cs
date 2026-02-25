using Microsoft.EntityFrameworkCore;
using PlayProjectify.OrderService.ApiClients;
using PlayProjectify.OrderService.Data;
using PlayProjectify.OrderService.Models.DTO;
using PlayProjectify.OrderService.Models.Entites;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.OrderService.Services;

public interface IOrderService
{
    Task<ProjectifyServiceResult<IEnumerable<OrderDto>>> GetAll(CancellationToken cancellationToken = default);
    Task<ProjectifyServiceResult<OrderDto>> Get(Guid id, CancellationToken cancellationToken = default);
    Task<ProjectifyServiceResult<OrderDto>> Add(CreateOrderDto newOrder, CancellationToken cancellationToken = default);
    Task<ProjectifyServiceResult> Cancel(Guid id, CancellationToken cancellationToken = default);
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

    public async Task<ProjectifyServiceResult<IEnumerable<OrderDto>>> GetAll(CancellationToken cancellationToken = default)
    {             
        return await _dbContext.Orders
            .Include(d => d.Items)
            .Select(o => new OrderDto(
                o.Id,
                o.CustomerId,
                o.OrderStatus.ToString(),
                o.TotalAmount,
                o.CreatedAt,
                o.Items.Select(d => new OrderItemDto(d.Id, d.ProductName, d.UnitPrice, d.Quantity))
                )
            )
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectifyServiceResult<OrderDto>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _dbContext.Orders
            .Include(d => d.Items)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (order is not null)
            return new OrderDto(order.Id, order.CustomerId, order.OrderStatus.ToString(), order.TotalAmount, order.CreatedAt, order.Items.Select(d => new OrderItemDto(d.Id, d.ProductName, d.UnitPrice, d.Quantity)));

        return ProjectifyServiceResult<OrderDto>.NotFound($"Order with ID {id} not found.");
    }

    public async Task<ProjectifyServiceResult<OrderDto>> Add(CreateOrderDto newOrder, CancellationToken cancellationToken = default)
    {
        var toInsert = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = newOrder.CustomerId,
            OrderStatus = OrderStatus.Pending,
        };

        foreach (var item in newOrder.Items)
        {
            var product = await _productApiClient.GetProduct(item.ProductId, cancellationToken);

            if (product == null)
                return ProjectifyServiceResult<OrderDto>.BadRequest($"Product with ID {item.ProductId} not found");

            if (product.ProductQuantity < item.Quantity)
                return ProjectifyServiceResult<OrderDto>.BadRequest($"Product with ID {item.ProductId} has insufficient stock");

            toInsert.Items.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.ProductPrice,
                Quantity = item.Quantity
            });
        }

        toInsert.TotalAmount = toInsert.Items.Sum(x => x.TotalPrice);
        _dbContext.Orders.Add(toInsert);
        var dbResult = await _dbContext.SaveChangesAsync(cancellationToken);
        if (dbResult == 0)
            return ProjectifyServiceResult<OrderDto>.CommonError("Failed to place the new order.");
        return new OrderDto(toInsert.Id, toInsert.CustomerId, toInsert.OrderStatus.ToString(), toInsert.TotalAmount, toInsert.CreatedAt, toInsert.Items.Select(d => new OrderItemDto(d.Id, d.ProductName, d.UnitPrice, d.Quantity)));
    }

    public async Task<ProjectifyServiceResult> Cancel(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Orders.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (existing is null) 
            return ProjectifyServiceResult<object>.NotFound("Order not found.");

        if (existing.OrderStatus != OrderStatus.Pending)
            return ProjectifyServiceResult<object>.BadRequest("Only pending orders can be cancelled.");

        existing.OrderStatus = OrderStatus.Cancelled;
        existing.UpdatedAt = DateTime.UtcNow;
        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        if (result == 0)
            return ProjectifyServiceResult<object>.CommonError("Failed to cancel the order.");
        else return ProjectifyServiceResult.Success();
    }

}
