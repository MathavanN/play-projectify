using Asp.Versioning;
using Asp.Versioning.Builder;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ApiService.Apis;

public static class WeatherApi
{
    public static IEndpointRouteBuilder MapWeatherApi(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewVersionedApi("Weather")
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        app.MapWeatherApiV1(versionSet);

        return app;
    }

    private static void MapWeatherApiV1(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var api = app.MapGroup("api/v{version:apiVersion}/weather")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1, 0);

        string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

        api.MapGet("/", () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithSummary("Get Weather Forecast")
        .WithDescription("Get all Weather Forecast")
        .WithTags("Weather")
        .Produces<ProjectifyServiceResult<IEnumerable<WeatherForecast>>>(200);
    }
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}