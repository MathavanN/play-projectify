using Microsoft.AspNetCore.Mvc;

namespace PlayProjectify.OrderService.ApiClients;

public interface IProductApiClient
{
    Task<Product?> GetProduct(Guid id, CancellationToken cancellationToken = default);
}

public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _httpClient;
    public ProductApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Product?> GetProduct(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _httpClient.GetFromJsonAsync<ApiClientResult<Product>>($"api/v1/product/{id}", cancellationToken);
        if (result is not null && result.IsSuccess)
            return result.Data;
        return null;
    }
}

public record Product(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice, int ProductQuantity, Guid CategoryId, string CategoryName);

public record ApiClientResult<T>
{
    public bool IsSuccess { get; init; }
    public ProblemDetails? Error { get; init; }
    public T? Data { get; init; }
}