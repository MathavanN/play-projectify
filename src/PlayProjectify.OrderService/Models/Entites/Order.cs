namespace PlayProjectify.OrderService.Models.Entites;

public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }

    public ICollection<OrderItem> Items { get; set; } = [];
}

public enum OrderStatus
{
    Pending,
    Paid,
    Shipped,
    Cancelled
}
