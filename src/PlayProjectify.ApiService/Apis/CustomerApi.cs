using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using PlayProjectify.ApiService.Models.DTO;
using PlayProjectify.ApiService.Services;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ApiService.Apis;

public static class CustomerApi
{
    public static IEndpointRouteBuilder MapCustomerApi(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewVersionedApi("Customer")
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        app.MapCustomerApiV1(versionSet);

        return app;
    }

    private static void MapCustomerApiV1(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var api = app.MapGroup("api/v{version:apiVersion}/Customer")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1, 0);


        api.MapGet("/", GetCustomersV1)
            .WithName("GetCustomers")
            .WithSummary("Get customers")
            .WithDescription("Get all customers")
            .WithTags("Customer")
            .Produces<ProjectifyServiceResult<IEnumerable<CustomerDto>>>(200);

        api.MapGet("/{id:guid}", GetCustomerV1)
            .WithName("GetCustomer")
            .WithSummary("Get an customer")
            .WithDescription("Get an customer")
            .WithTags("Customer")
            .Produces<ProjectifyServiceResult<CustomerDto>>(200)
            .Produces<ProjectifyServiceResult>(404);

        api.MapPatch("/{id:guid}", PatchCustomerV1)
            .WithName("PatchCustomer")
            .WithSummary("Partially update a customer")
            .WithTags("Customer")
            .Accepts<JsonPatchDocument<PatchCustomerDto>>("application/json-patch+json")
            .Produces<ProjectifyServiceResult<CustomerDto>>(200)
            .Produces<ProjectifyServiceResult>(400)
            .Produces<ProjectifyServiceResult>(404);

    }
    private static async Task<IResult> GetCustomersV1(ICustomerService customerService, CancellationToken cancellationToken)
    {
        var result = await customerService.GetAll(cancellationToken);
        return result.ToApiResult();
    }
    private static async Task<IResult> GetCustomerV1(ICustomerService customerService, Guid id, CancellationToken cancellationToken)
    {
        var result = await customerService.Get(id, cancellationToken);
        return result.ToApiResult();
    }
    private static async Task<IResult> PatchCustomerV1(ICustomerService customerService, Guid id, JsonPatchDocument<PatchCustomerDto> patchDoc, CancellationToken cancellationToken)
    {
        var result = await customerService.Patch(id, patchDoc, cancellationToken);
        return result.ToApiResult();
    }
}