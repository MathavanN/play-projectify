using Microsoft.AspNetCore.Mvc;

namespace PlayProjectify.Web;

public class ProductApiClient(HttpClient httpClient)
{
    public async Task<ProductDto[]> GetProductsAsync(CancellationToken cancellationToken = default)
    {

        var result = await httpClient.GetFromJsonAsync<ApiResult<ProductDto[]>>("api/v1/product", cancellationToken);
        if (result is not null && result.IsSuccess)
            return result.Data ?? [];
        return [];
    }

    public async Task<ProductDto?> GetProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var result = await httpClient.GetFromJsonAsync<ApiResult<ProductDto>>($"api/v1/product/{productId}", cancellationToken);
        if (result is not null && result.IsSuccess)
            return result.Data;
        return null;
    }
}

public record ProductDto(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice);
public record ApiResult<T>
{
    public bool IsSuccess { get; init; }
    public ProblemDetails? Error { get; init; }
    public T? Data { get; init; }
}