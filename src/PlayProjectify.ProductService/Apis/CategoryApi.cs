using Asp.Versioning;
using Asp.Versioning.Builder;
using PlayProjectify.ProductService.Models.DTO;
using PlayProjectify.ProductService.Services;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ProductService.Apis;

public static class CategoryApi
{
    public static IEndpointRouteBuilder MapCategoryApi(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewVersionedApi("Category")
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        app.MapCategoryApiV1(versionSet);

        return app;
    }

    private static void MapCategoryApiV1(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var api = app.MapGroup("api/v{version:apiVersion}/category")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1, 0);

        api.MapGet("/", GetCategoriesV1)
            .WithName("GetCategories")
            .WithSummary("Get categories")
            .WithDescription("Get all categories")
            .WithTags("Category")
            .Produces<ProjectifyServiceResult<IEnumerable<CategoryDto>>>(200);

        api.MapGet("/{id:guid}", GetCategoryV1)
            .WithName("GetCategory")
            .WithSummary("Get a category")
            .WithDescription("Get a category")
            .WithTags("Category")
            .Produces<ProjectifyServiceResult<CategoryDto>>(200)
            .Produces<ProjectifyServiceResult>(404);

        api.MapPost("/", AddCategoryV1)
            .WithName("AddCategory")
            .WithSummary("Add category")
            .WithDescription("Add new category")
            .WithTags("Category")
            .Validate<AddCategoryDto>()
            .Produces<ProjectifyServiceResult<CategoryDto>>(201)
            .Produces<ProjectifyServiceResult>(400);
    }

    private static async Task<IResult> GetCategoriesV1(ICategoryService categoryService)
    {
        var result = await categoryService.GetAll();
        return result.ToApiResult();
    }

    private static async Task<IResult> GetCategoryV1(ICategoryService categoryService, Guid id)
    {
        var result = await categoryService.Get(id);
        return result.ToApiResult();
    }

    private static async Task<IResult> AddCategoryV1(ICategoryService categoryService, AddCategoryDto category)
    {
        var result = await categoryService.Add(category);
        return result.ToApiResult("GetCategory", data => new { id = data.CategoryId });
    }
}