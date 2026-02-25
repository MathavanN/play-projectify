using Microsoft.EntityFrameworkCore;
using PlayProjectify.ProductService.Data;
using PlayProjectify.ProductService.Models.DTO;
using PlayProjectify.ProductService.Models.Entites;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ProductService.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ProductDbContext _dbContext;
    public CategoryService(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProjectifyServiceResult<CategoryDto>> Add(AddCategoryDto category, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Categories.FirstOrDefaultAsync(d => string.Equals(d.Name, category.CategoryName, StringComparison.OrdinalIgnoreCase), cancellationToken);

        if (existing != null)
            return new CategoryDto(existing.Id, existing.Name, existing.Description);

        var toInsert = new Category()
        {
            Name = category.CategoryName,
            Description = category.CategoryDescription
        };

        _dbContext.Categories.Add(toInsert);
        var dbResult = await _dbContext.SaveChangesAsync(cancellationToken);
        if (dbResult == 0)
            return ProjectifyServiceResult<CategoryDto>.CommonError("Failed to save the new category.");
        return new CategoryDto(toInsert.Id, toInsert.Name, toInsert.Description);
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Categories.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (existing is null) return false;
        _dbContext.Categories.Remove(existing);
        return (await _dbContext.SaveChangesAsync(cancellationToken)) == 1;
    }

    public async Task<ProjectifyServiceResult<CategoryDto>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (category is not null)
            return new CategoryDto(category.Id, category.Name, category.Description);

        return ProjectifyServiceResult<CategoryDto>.NotFound($"Category with ID {id} not found");
    }

    public async Task<ProjectifyServiceResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories
            .OrderBy(p => p.Id)
            .Select(p => new CategoryDto(p.Id, p.Name, p.Description))
            .ToListAsync(cancellationToken);
    }

    public Task<bool> Update(UpdateCategoryDto category, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}