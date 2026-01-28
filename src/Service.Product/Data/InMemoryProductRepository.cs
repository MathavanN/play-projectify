using PlayProjectify.Core.ApiProblemBuilder;
using PlayProjectify.Core.ApiResult;
using System.Collections.Generic;

namespace Service.Product.Data;
public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly ProductStore _store;
    private readonly IApiProblemBuilder _problemBuilder;
    public InMemoryProductRepository(IApiProblemBuilder problemBuilder, ProductStore store)
    {
        _store = store;
        _problemBuilder = problemBuilder;
    }

    public ApiResult<IEnumerable<Product>> GetAll() => _store.Store.Values.OrderBy(p => p.Id).ToList();

    public ApiResult<Product> Get(int id) {
        return _store.Store.TryGetValue(id, out var p) ? p : ApiResult<Product>.NotFound(_problemBuilder, $"Product with ID {id} not found");
   }

    public Product Add(Product product)
    {
        // Simple auto-id for demo; in real code consider a proper id generator.
        var id = _store.Store.Keys.DefaultIfEmpty(0).Max() + 1;
        var toInsert = product with { Id = id };
        _store.Store[id] = toInsert;
        return toInsert;
    }

    public bool Update(Product product)
    {
        if (!_store.Store.ContainsKey(product.Id)) return false;
        _store.Store[product.Id] = product;
        return true;
    }

    public bool Delete(int id) => _store.Store.TryRemove(id, out _);
}
