using PlayProjectify.ProductService.Data;
using PlayProjectify.ProductService.Models.DTO;
using PlayProjectify.ProductService.Models.Entites;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ProductService.Services;

public sealed class InMemoryProductService : IProductService
{
    private readonly ProductStore _store;
    public InMemoryProductService(ProductStore store)
    {
        _store = store;
    }

    public ProjectifyServiceResult<IEnumerable<ProductDto>> GetAll()
    {
        return _store.Store.Values
            .OrderBy(p => p.Id)
            .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price))
            .ToList();
    }

    public ProjectifyServiceResult<ProductDto> Get(Guid id)
    {
        if (_store.Store.TryGetValue(id, out var p))
            return new ProductDto(p.Id, p.Name, p.Description, p.Price);

        return ProjectifyServiceResult<ProductDto>.NotFound($"Product with ID {id} not found");
    }


    public ProjectifyServiceResult<ProductDto> Add(AddProductDto product)
    {
        var existing = _store.Store.Values.FirstOrDefault(d => string.Equals(d.Name, product.ProductName, StringComparison.OrdinalIgnoreCase));

        if (existing != null)
            return new ProductDto(existing.Id, existing.Name, existing.Description, existing.Price);

        var toInsert = new Product(Guid.NewGuid(), product.ProductName, product.ProductDescription, product.ProductPrice);
        _store.Store.TryAdd(toInsert.Id, toInsert);
        return new ProductDto(toInsert.Id, toInsert.Name, toInsert.Description, toInsert.Price);
    }

    public bool Update(UpdateProductDto product)
    {
        if (!_store.Store.ContainsKey(product.ProductId)) return false;
        _store.Store[product.ProductId] = new Product(product.ProductId, product.ProductName, product.ProductDescription, product.ProductPrice);
        return true;
    }

    public bool Delete(Guid id) => _store.Store.TryRemove(id, out _);


}