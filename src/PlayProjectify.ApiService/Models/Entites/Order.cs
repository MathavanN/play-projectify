namespace PlayProjectify.ApiService.Models.Entites;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public sealed class Order : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    // Foreign Keys
    public Guid ShippingAddressId { get; set; }
    public Guid? BillingAddressId { get; set; }

    // Navigation Properties
    public Address ShippingAddress { get; set; } = null!;
    public Address? BillingAddress { get; set; }

    public DateTime OrderDate { get; set; }

    public List<OrderItem> Items { get; set; } = [];

    public decimal TotalAmount { get; set; }

    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
}

public sealed class Address : BaseEntity
{
    public string Street1 { get; set; } = null!;
    public string? Street2 { get; set; }
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
}

public sealed class OrderItem : BaseEntity
{
    // Foreign Key to Order
    public Guid OrderId { get; set; }

    // Navigation Property
    public Order Order { get; set; } = null!;

    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice => Quantity * UnitPrice;
}

public sealed class Customer : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? CompanyName { get; set; }
    public string? Notes { get; set; }
    public string PreferredCurrency { get; set; } = "CHF";
    public string PreferredLanguage { get; set; } = "en";
    public bool MarketingOptIn { get; set; } = false;
    public bool IsActive { get; set; } = true;
}
public enum OrderStatus
{
    Pending,
    Paid,
    Shipped,
    Cancelled
}