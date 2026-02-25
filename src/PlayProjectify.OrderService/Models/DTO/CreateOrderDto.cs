namespace PlayProjectify.OrderService.Models.DTO;

public class CreateOrderDto
{
    public Guid CustomerId { get; set; }
    public List<CreateOrderItemDto> Items { get; set; } = [];
}

public class CreateOrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public sealed record OrderDto(Guid OrderId, Guid CustomerId, string OrderStatus, decimal TotalAmount, DateTime CreatedAt, IEnumerable<OrderItemDto> Items);

public sealed record OrderItemDto(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity)
{
    public decimal TotalPrice => UnitPrice * Quantity;
};
