namespace PlayProjectify.ApiService.Models.DTO;

public sealed record OrderDto(Guid OrderId, 
    string FirstName, 
    string LastName, 
    string Email, 
    string PhoneNumber,
    Guid ShippingAddressId,
    AddressDto ShippingAddress,
    Guid? BillingAddressId,
    AddressDto? BillingAddress,
    DateTime OrderDate,
    List<OrderItemDto> Items,
    decimal TotalAmount,
    string OrderStatus,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );

public sealed record OrderItemDto(Guid ItemId, string ProductName, decimal UnitPrice, int Quantity)
{
    public decimal TotalPrice => UnitPrice * Quantity;
};

public sealed record AddressDto(Guid AddressId, string Street1, string? Street2, string City, string State, string PostalCode, string Country);
