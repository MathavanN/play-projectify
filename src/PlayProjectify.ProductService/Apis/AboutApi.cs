using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PlayProjectify.ProductService.Apis;

public static class AboutApi
{
    public static IEndpointRouteBuilder MapAboutApi(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewVersionedApi("About")
            .NewApiVersionSet()
                    .HasApiVersion(new ApiVersion(1.0))
                    .HasApiVersion(new ApiVersion(2.0))
                    .ReportApiVersions()
                    .Build();

        app.MapAboutApiV1(versionSet);
        app.MapAboutApiV2(versionSet);

        return app;
    }
    private static void MapAboutApiV1(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var api = app.MapGroup("api/v{version:apiVersion}/about")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1, 0);
        api.MapGet("/", GetAboutV1)
            .WithSummary("Get about")
            .WithDescription("Get about")
            .WithTags("About")
            .Produces<string>(200);
    }

    private static void MapAboutApiV2(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var api = app.MapGroup("api/v{version:apiVersion}/about")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(2, 0);
        api.MapGet("/", GetAboutV2)
            .WithSummary("Get about")
            .WithDescription("Get about")
            .WithTags("About")
            .Produces<string>(200);
    }

    private static async Task<Ok<string>> GetAboutV1()
    {
        return TypedResults.Ok("This is Product service API v1");
    }

    private static async Task<Ok<string>> GetAboutV2()
    {
        return TypedResults.Ok($"This is Product service API v2 at {DateTime.Now}");
    }
}
