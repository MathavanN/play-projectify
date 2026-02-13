namespace PlayProjectify.OrderService.Models.Entites;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    // Reference only
    public Guid ProductId { get; set; }

    // Snapshot from Product service
    public string ProductName { get; set; } = null!;
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice => UnitPrice * Quantity;
}
