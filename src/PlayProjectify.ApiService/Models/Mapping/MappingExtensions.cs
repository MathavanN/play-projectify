using PlayProjectify.ApiService.Models.DTO;
using PlayProjectify.ApiService.Models.Entites;
using PlayProjectify.ApiService.Models.Mapping;

namespace PlayProjectify.ApiService.Models.Mapping;

public static class MappingExtensions
{
    extension(Customer d)
    {
        public CustomerDto ToDto() => new(
        d.Id,
        d.FirstName,
        d.LastName,
        d.Email,
        d.PhoneNumber,
        d.CompanyName,
        d.Notes,
        d.PreferredCurrency,
        d.PreferredLanguage,
        d.MarketingOptIn,
        d.IsActive,
        d.CreatedAt,
        d.UpdatedAt
    );
    }

    extension(Order d)
    {
        public OrderDto ToDto() => new(
        d.Id,
        d.FirstName,
        d.LastName,
        d.Email,
        d.PhoneNumber,
        d.ShippingAddressId,
        d.ShippingAddress.ToDto(),
        d.BillingAddressId,
        d.BillingAddress?.ToDto(),
        d.OrderDate,
        [.. d.Items.Select(d => d.ToDto())],
        d.TotalAmount,
        d.OrderStatus.ToString(),
        d.CreatedAt,
        d.UpdatedAt
    );
    }

    extension(Address d)
    {
        public AddressDto ToDto() => new(d.Id, d.Street1, d.Street2, d.City, d.State, d.PostalCode, d.Country);
    }

    extension(OrderItem d)
    {
        public OrderItemDto ToDto() => new(d.Id, d.ProductName, d.UnitPrice, d.Quantity);
    }
}
