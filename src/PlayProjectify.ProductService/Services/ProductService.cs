using Microsoft.EntityFrameworkCore;
using PlayProjectify.ProductService.Data;
using PlayProjectify.ProductService.Models.DTO;
using PlayProjectify.ProductService.Models.Entites;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ProductService.Services;

public sealed class ProductService : IProductService
{
    private readonly ProductDbContext _dbContext;
    public ProductService(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProjectifyServiceResult<IEnumerable<GetProductDto>>> GetAll()
    {
        return await _dbContext.Products
            .Include(p => p.Category)
            .OrderBy(p => p.Id)
            .Select(p => new GetProductDto(p.Id, p.Name, p.Description, p.Price, p.StockQuantity, p.CategoryId, p.Category != null ? p.Category.Name : "Unknown"))
            .ToListAsync();
    }

    public async Task<ProjectifyServiceResult<GetProductDto>> Get(Guid id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product is not null)
            return new GetProductDto(product.Id, product.Name, product.Description, product.Price, product.StockQuantity, product.CategoryId, product.Category != null ? product.Category.Name : "Unknown");

        return ProjectifyServiceResult<GetProductDto>.NotFound($"Product with ID {id} not found");
    }


    public async Task<ProjectifyServiceResult<ProductDto>> Add(AddProductDto product)
    {
        var existing = _dbContext.Products.FirstOrDefault(d => string.Equals(d.Name, product.ProductName, StringComparison.OrdinalIgnoreCase));

        if (existing != null)
            return new ProductDto(existing.Id, existing.Name, existing.Description, existing.Price, existing.StockQuantity, existing.CategoryId);

        var toInsert = new Product()
        {
            Id = Guid.NewGuid(),
            Name = product.ProductName,
            Description = product.ProductDescription,
            Price = product.ProductPrice,
            StockQuantity = product.ProductQuantity,
            CategoryId = product.CategoryId,
        };
        _dbContext.Products.Add(toInsert);
        var dbResult = await _dbContext.SaveChangesAsync();
        if (dbResult == 0)
            return ProjectifyServiceResult<ProductDto>.CommonError("Failed to save the new product.");
        return new ProductDto(toInsert.Id, toInsert.Name, toInsert.Description, toInsert.Price, toInsert.StockQuantity, toInsert.CategoryId);
    }

    public async Task<bool> Update(UpdateProductDto product)
    {
        var existing = _dbContext.Products.FirstOrDefault(p => p.Id == product.ProductId);

        if (existing is null) return false;

        // Update only changed properties
        bool isModified = false;

        if (!string.IsNullOrWhiteSpace(product.ProductName) && existing.Name != product.ProductName)
        {
            existing.Name = product.ProductName;
            isModified = true;
        }

        if (!string.IsNullOrWhiteSpace(product.ProductDescription) && existing.Description != product.ProductDescription)
        {
            existing.Description = product.ProductDescription;
            isModified = true;
        }

        if (existing.Price != product.ProductPrice)
        {
            existing.Price = product.ProductPrice;
            isModified = true;
        }

        if (existing.StockQuantity != product.ProductQuantity)
        {
            existing.StockQuantity = product.ProductQuantity;
            isModified = true;
        }

        if (existing.CategoryId != product.CategoryId)
        {
            existing.CategoryId = product.CategoryId;
            isModified = true;
        }

        if (isModified)
        {
            existing.UpdatedAt = DateTime.UtcNow;
            return (await _dbContext.SaveChangesAsync()) == 1;
        }
        else return isModified;
    }

    public async Task<bool> Delete(Guid id)
    {
        var existing = _dbContext.Products.FirstOrDefault(p => p.Id == id);
        if (existing is null) return false;
        _dbContext.Products.Remove(existing);
        return (await _dbContext.SaveChangesAsync()) == 1;
    }
}