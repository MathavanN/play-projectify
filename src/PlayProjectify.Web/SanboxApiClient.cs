using System.Text;
using System.Text.Json;

namespace PlayProjectify.Web;

public class SanboxApiClient(HttpClient httpClient)
{
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>("api/v1/weather", cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<GetCustomerDto[]> GetCustomersAsync(CancellationToken cancellationToken = default)
    {

        var result = await httpClient.GetFromJsonAsync<ApiResult<GetCustomerDto[]>>("api/v1/Customer", cancellationToken);
        if (result is not null && result.IsSuccess)
            return result.Data ?? [];
        return [];
    }
    public async Task<GetCustomerDto?> GetCustomerAsync(Guid id, CancellationToken ct = default)
    {
        var result = await httpClient.GetFromJsonAsync<ApiResult<GetCustomerDto>>($"api/v1/Customer/{id}", ct);
        if (result is not null && result.IsSuccess)
            return result.Data;
        return null;
    }

    public async Task<GetCustomerDto?> PatchCustomerAsync(Guid id, List<JsonPatchOperation> operations, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(operations, JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");

        var response = await httpClient.PatchAsync($"api/v1/Customer/{id}", content, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResult<GetCustomerDto>>(JsonOptions, ct);
        if (result is not null && result.IsSuccess)
            return result.Data;
        return null;
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public sealed record JsonPatchOperation(string Op, string Path, object? Value);

public sealed record GetCustomerDto(
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

public sealed class CustomerEditModel
{
    public string? PhoneNumber { get; set; }
    public string? CompanyName { get; set; }
    public string? Notes { get; set; }
    public string PreferredCurrency { get; set; } = "CHF";
    public string PreferredLanguage { get; set; } = "en";
    public bool MarketingOptIn { get; set; }
    public bool IsActive { get; set; }

    public static CustomerEditModel FromDto(GetCustomerDto dto) => new()
    {
        PhoneNumber = dto.PhoneNumber,
        CompanyName = dto.CompanyName,
        Notes = dto.Notes,
        PreferredCurrency = dto.PreferredCurrency,
        PreferredLanguage = dto.PreferredLanguage,
        MarketingOptIn = dto.MarketingOptIn,
        IsActive = dto.IsActive
    };

    // Build only the operations that actually changed
    public List<JsonPatchOperation> BuildPatch(GetCustomerDto original)
    {
        var ops = new List<JsonPatchOperation>();

        if (PhoneNumber != original.PhoneNumber)
            ops.Add(PhoneNumber is null
                ? new("remove", "/phoneNumber", null)
                : new("replace", "/phoneNumber", PhoneNumber));

        if (CompanyName != original.CompanyName)
            ops.Add(CompanyName is null
                ? new("remove", "/companyName", null)
                : new("replace", "/companyName", CompanyName));

        if (Notes != original.Notes)
            ops.Add(Notes is null
                ? new("remove", "/notes", null)
                : new("replace", "/notes", Notes));

        if (PreferredCurrency != original.PreferredCurrency)
            ops.Add(new("replace", "/preferredCurrency", PreferredCurrency));

        if (PreferredLanguage != original.PreferredLanguage)
            ops.Add(new("replace", "/preferredLanguage", PreferredLanguage));

        if (MarketingOptIn != original.MarketingOptIn)
            ops.Add(new("replace", "/marketingOptIn", MarketingOptIn));

        if (IsActive != original.IsActive)
            ops.Add(new("replace", "/isActive", IsActive));

        return ops;
    }
}