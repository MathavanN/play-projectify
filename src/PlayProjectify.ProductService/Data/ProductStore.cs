using PlayProjectify.ProductService.Models.Entites;
using System.Collections.Concurrent;

namespace PlayProjectify.ProductService.Data;

public sealed class ProductStore
{
    // This is the single source of truth for all products
    public ConcurrentDictionary<Guid, Product> Store { get; }

    public ProductStore()
    {
        Store = new ConcurrentDictionary<Guid, Product>();

        // Seed initial data
        var products = new List<Product>
    {
        new(Guid.NewGuid(), "PlayStation 5", "Sony next-gen gaming console", 499.99m),
        new(Guid.NewGuid(), "Xbox Series X", "Microsoft gaming console", 479.99m),
        new(Guid.NewGuid(), "Canon EOS R50", "Mirrorless camera", 699.00m),
        new(Guid.NewGuid(), "Dell XPS 13", "Ultrabook laptop", 1299.00m),
        new(Guid.NewGuid(), "Dyson V12", "Cordless vacuum cleaner", 599.00m)
    };

        // Add to ConcurrentDictionary
        foreach (var p in products)
            Store.TryAdd(p.Id, p);
    }
}
