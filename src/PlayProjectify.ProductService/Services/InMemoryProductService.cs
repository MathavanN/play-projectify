using PlayProjectify.ProductService.Data;
using PlayProjectify.ProductService.Models.DTOs;
using PlayProjectify.ProductService.Models.Entites;

namespace PlayProjectify.ProductService.Services;

public sealed class InMemoryProductService : IProductService
{
    private readonly ProductStore _store;
    public InMemoryProductService(ProductStore store)
    {
        _store = store;
    }

    public IEnumerable<ProductDto> GetAll()
    {
        return _store.Store.Values.OrderBy(p => p.Id).Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price)); ;
    }

    public ProductDto? Get(Guid id)
    {
        return _store.Store.TryGetValue(id, out var p) ?
            new ProductDto(p.Id, p.Name, p.Description, p.Price)
            : null;
    }

    public ProductDto Add(AddProductDto product)
    {
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
