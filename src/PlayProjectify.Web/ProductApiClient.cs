using Microsoft.AspNetCore.Mvc;

namespace PlayProjectify.Web;

public class ProductApiClient(HttpClient httpClient)
{
    public async Task<GetProductDto[]> GetProductsAsync(CancellationToken cancellationToken = default)
    {

        var result = await httpClient.GetFromJsonAsync<ApiResult<GetProductDto[]>>("api/v1/product", cancellationToken);
        if (result is not null && result.IsSuccess)
            return result.Data ?? [];
        return [];
    }

    public async Task<GetProductDto?> GetProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var result = await httpClient.GetFromJsonAsync<ApiResult<GetProductDto>>($"api/v1/product/{productId}", cancellationToken);
        if (result is not null && result.IsSuccess)
            return result.Data;
        return null;
    }
}

public class OrderApiClient(HttpClient httpClient)
{
    public async Task CreateOrderAsync(CreateOrderDto order, CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync("api/v1/order", order, cancellationToken);
    }
    public async Task<GetOrderDto?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var result = await httpClient.GetFromJsonAsync<ApiResult<GetOrderDto>>($"api/v1/order/{orderId}", cancellationToken);
        if (result is not null && result.IsSuccess)
            return result.Data;
        return null;
    }
}

public record CreateOrderDto(Guid CustomerId, IEnumerable<CreateOrderItemDto> Items);
public record CreateOrderItemDto(Guid ProductId, int Quantity);
public record GetOrderDto(Guid OrderId, Guid CustomerId, string OrderStatus, decimal TotalAmount, DateTime CreatedAt, IEnumerable<OrderItemDto> Items);
public record OrderItemDto(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity)
{
    public decimal TotalPrice => UnitPrice * Quantity;
};

public record GetProductDto(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice, int ProductQuantity, Guid CategoryId, string CategoryName);

public record ApiResult<T>
{
    public bool IsSuccess { get; init; }
    public ProblemDetails? Error { get; init; }
    public T? Data { get; init; }
}