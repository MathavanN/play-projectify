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

    public async Task<ProjectifyServiceResult<IEnumerable<GetProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .Include(p => p.Category)
            .OrderBy(p => p.Id)
            .Select(p => new GetProductDto(p.Id, p.Name, p.Description, p.Price, p.StockQuantity, p.CategoryId, p.Category != null ? p.Category.Name : "Unknown"))
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectifyServiceResult<ProductLookupDto>> GetByIds(IEnumerable<Guid> productIds, CancellationToken cancellationToken)
    {
        var ids = productIds
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToList();

        var products = await _dbContext.Products
            .Where(d => ids.Contains(d.Id))
            .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.StockQuantity, p.CategoryId))
            .ToListAsync(cancellationToken);

        var foundIds = products.Select(p => p.ProductId).ToHashSet();
        var notFoundIds = ids
            .Where(id => !foundIds.Contains(id))
            .ToList();
        return new ProductLookupDto(products, notFoundIds);
    }

    public async Task<ProjectifyServiceResult<GetProductDto>> Get(Guid id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (product is not null)
            return new GetProductDto(product.Id, product.Name, product.Description, product.Price, product.StockQuantity, product.CategoryId, product.Category != null ? product.Category.Name : "Unknown");

        return ProjectifyServiceResult<GetProductDto>.NotFound($"Product with ID {id} not found");
    }


    public async Task<ProjectifyServiceResult<ProductDto>> Add(AddProductDto product, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Products.FirstOrDefaultAsync(d => string.Equals(d.Name, product.ProductName, StringComparison.OrdinalIgnoreCase), cancellationToken);

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
        var dbResult = await _dbContext.SaveChangesAsync(cancellationToken);
        if (dbResult == 0)
            return ProjectifyServiceResult<ProductDto>.CommonError("Failed to save the new product.");
        return new ProductDto(toInsert.Id, toInsert.Name, toInsert.Description, toInsert.Price, toInsert.StockQuantity, toInsert.CategoryId);
    }

    public async Task<bool> Update(UpdateProductDto product, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == product.ProductId, cancellationToken);

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
            return (await _dbContext.SaveChangesAsync(cancellationToken)) == 1;
        }
        else return isModified;
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (existing is null) return false;
        _dbContext.Products.Remove(existing);
        return (await _dbContext.SaveChangesAsync(cancellationToken)) == 1;
    }

    public async Task<ProjectifyServiceResult> ReserveStock(ProductInventoryDto items, CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products
            .Where(p => items.Items.Select(i => i.ProductId).Contains(p.Id))
            .ToListAsync(cancellationToken);
        foreach (var item in items.Items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product is null)
                return ProjectifyServiceResult<object>.NotFound($"Product with ID {item.ProductId} not found");

            if (product.StockQuantity < item.Quantity)
                return ProjectifyServiceResult<object>.BadRequest($"Product with ID {item.ProductId} has insufficient stock");

            product.StockQuantity -= item.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
        }

        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        if (result == 0)
            return ProjectifyServiceResult<object>.CommonError("Failed to reserve the stock.");
        else return ProjectifyServiceResult.Success();
    }

    public async Task<ProjectifyServiceResult> ReleaseStock(ProductInventoryDto items, CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products
            .Where(p => items.Items.Select(i => i.ProductId).Contains(p.Id))
            .ToListAsync(cancellationToken);
        foreach (var item in items.Items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product is null)
                return ProjectifyServiceResult<object>.NotFound($"Product with ID {item.ProductId} not found");

            product.StockQuantity += item.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
        }

        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        if (result == 0)
            return ProjectifyServiceResult<object>.CommonError("Failed to release the stock.");
        else return ProjectifyServiceResult.Success();
    }
}