using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PlayProjectify.OrderService.ApiClients;

public interface IProductApiClient
{
    Task<ProductResult?> GetProduct(Guid id, CancellationToken cancellationToken);
    Task<ProductLookupResult> GetProductLookup(IEnumerable<Guid> productIds, CancellationToken cancellationToken);
    Task<ApiClientResult> ReserveInventoryStock(IEnumerable<ProductStockRequest> items, CancellationToken cancellationToken);
    Task<ApiClientResult> ReleaseInventoryStock(IEnumerable<ProductStockRequest> items, CancellationToken cancellationToken);
}

public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _httpClient;
    public ProductApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromMinutes(5);
    }

    public async Task<ProductResult?> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var result = await _httpClient.GetFromJsonAsync<ApiClientResult<ProductResult>>($"api/v1/product/{id}", cancellationToken);
        if (result is not null && result.IsSuccess)
            return result.Data;
        return null;
    }

    public async Task<ProductLookupResult> GetProductLookup(IEnumerable<Guid> productIds, CancellationToken cancellationToken)
    {
        var ids = productIds?
        .Where(x => x != Guid.Empty)
        .Distinct()
        .ToList() ?? [];

        if (ids.Count == 0)
            return new ProductLookupResult([], []);

        using var response = await _httpClient.PostAsJsonAsync("/api/v1/product/lookupByIds", new ProductLookupRequest(ids), cancellationToken);
        if (!response.IsSuccessStatusCode)
            return new ProductLookupResult([], ids);

        var result = await response.Content.ReadFromJsonAsync<ApiClientResult<ProductLookupResult>>(cancellationToken);
        if (result is null || !result.IsSuccess || result.Data is null)
            return new ProductLookupResult([], ids);

        return result.Data;
    }

    public async Task<ApiClientResult> ReserveInventoryStock(IEnumerable<ProductStockRequest> items, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PutAsJsonAsync("/api/v1/product/reserve", new ProductInventoryRequest(items), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NoContent)
            return new ApiClientResult(true, null);

        var result = await response.Content.ReadFromJsonAsync<ApiClientResult>(cancellationToken) ?? new ApiClientResult(false, null);
        return result;
    }

    public async Task<ApiClientResult> ReleaseInventoryStock(IEnumerable<ProductStockRequest> items, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PutAsJsonAsync("/api/v1/product/release", new ProductInventoryRequest(items), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NoContent)
            return new ApiClientResult(true, null);

        var result = await response.Content.ReadFromJsonAsync<ApiClientResult>(cancellationToken) ?? new ApiClientResult(false, null);
        return result;
    }
}

public record ProductStockRequest(Guid ProductId, int Quantity);
public record ProductInventoryRequest(IEnumerable<ProductStockRequest> Items);

public record ProductLookupResult(IEnumerable<ProductResult> FoundProducts, IEnumerable<Guid> NotFoundProductIds);
public record ProductLookupRequest(IEnumerable<Guid> ProductIds);
public record ProductResult(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice, int ProductQuantity, Guid CategoryId, string CategoryName);

public record ApiClientResult<T>
{
    public bool IsSuccess { get; init; }
    public ProblemDetails? Error { get; init; }
    public T? Data { get; init; }
}
public record ApiClientResult(bool IsSuccess, ProblemDetails? Error);