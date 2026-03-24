using Microsoft.EntityFrameworkCore;
using PlayProjectify.ApiService.Data;
using PlayProjectify.ApiService.Models.DTO;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ApiService.Services;

public interface IOrderService
{
    Task<ProjectifyServiceResult<IEnumerable<OrderDto>>> GetAll(CancellationToken cancellationToken);
    Task<ProjectifyServiceResult<OrderDto>> Get(Guid id, CancellationToken cancellationToken);
}

public sealed class OrderService : IOrderService
{
    private readonly AppDbContext _dbContext;
    public OrderService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProjectifyServiceResult<IEnumerable<OrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        return await _dbContext.Orders
            .Include(p => p.ShippingAddress)
            .Include(p => p.BillingAddress)
            .Include(p => p.Items)
            .OrderBy(p => p.Id)
            .Select(p => new OrderDto(
                p.Id,
                p.FirstName,
                p.LastName,
                p.Email,
                p.PhoneNumber,
                p.ShippingAddressId,
                new AddressDto(p.ShippingAddress.Id, p.ShippingAddress.Street1, p.ShippingAddress.Street2, p.ShippingAddress.City, p.ShippingAddress.State, p.ShippingAddress.PostalCode, p.ShippingAddress.Country),
                p.BillingAddressId,
                p.BillingAddress == null ? null : new AddressDto(p.BillingAddress.Id, p.BillingAddress.Street1, p.BillingAddress.Street2, p.BillingAddress.City, p.BillingAddress.State, p.BillingAddress.PostalCode, p.BillingAddress.Country),
                p.OrderDate,
                p.Items.Select(d => new OrderItemDto(d.Id, d.ProductName, d.UnitPrice, d.Quantity)).ToList(),
                p.TotalAmount,
                p.OrderStatus.ToString(),
                p.CreatedAt,
                p.UpdatedAt
                ))
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectifyServiceResult<OrderDto>> Get(Guid id, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .Include(p => p.ShippingAddress)
            .Include(p => p.BillingAddress)
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (order is not null)
            return new OrderDto(
                order.Id,
                order.FirstName,
                order.LastName,
                order.Email,
                order.PhoneNumber,
                order.ShippingAddressId,
                new AddressDto(order.ShippingAddress.Id, order.ShippingAddress.Street1, order.ShippingAddress.Street2, order.ShippingAddress.City, order.ShippingAddress.State, order.ShippingAddress.PostalCode, order.ShippingAddress.Country),
                order.BillingAddressId,
                order.BillingAddress == null ? null : new AddressDto(order.BillingAddress.Id, order.BillingAddress.Street1, order.BillingAddress.Street2, order.BillingAddress.City, order.BillingAddress.State, order.BillingAddress.PostalCode, order.BillingAddress.Country),
                order.OrderDate,
                [.. order.Items.Select(d => new OrderItemDto(d.Id, d.ProductName, d.UnitPrice, d.Quantity))],
                order.TotalAmount,
                order.OrderStatus.ToString(),
                order.CreatedAt,
                order.UpdatedAt
                );

        return ProjectifyServiceResult<OrderDto>.NotFound($"Order with ID {id} not found.");
    }

    //private static OrderDto MapToDto(Order o)
    //{
    //    AddressDto MapAddress(Models.Address? addr) =>
    //        addr == null ? null :
    //        new AddressDto(
    //            addr.Id,
    //            addr.Street1,
    //            addr.Street2,
    //            addr.City,
    //            addr.State,
    //            addr.PostalCode,
    //            addr.Country
    //        );

    //    var items = o.Items.Select(i => new OrderItemDto(
    //        i.Id,
    //        i.ProductName,
    //        i.UnitPrice,
    //        i.Quantity
    //    )).ToList();

    //    return new OrderDto(
    //        o.Id,
    //        o.FirstName,
    //        o.LastName,
    //        o.Email,
    //        o.PhoneNumber,
    //        o.ShippingAddressId,
    //        MapAddress(o.ShippingAddress),
    //        o.BillingAddressId,
    //        MapAddress(o.BillingAddress),
    //        o.OrderDate,
    //        items,
    //        o.TotalAmount,
    //        o.OrderStatus.ToString(),
    //        o.CreatedAt,
    //        o.UpdatedAt
    //    );
    //}
}