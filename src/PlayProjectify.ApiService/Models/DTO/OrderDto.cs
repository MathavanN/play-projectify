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

public sealed class PatchCustomerDto
{
    public string? PhoneNumber { get; set; }
    public string? CompanyName { get; set; }
    public string? Notes { get; set; }
    public string? PreferredCurrency { get; set; }
    public string? PreferredLanguage { get; set; }
    public bool? MarketingOptIn { get; set; }
    public bool? IsActive { get; set; }
}

public sealed record CustomerDto(
    Guid CustomerId,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string? CompanyName,
    string? Notes,
    string PreferredCurrency,
    string PreferredLanguage,
    bool MarketingOptIn,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);