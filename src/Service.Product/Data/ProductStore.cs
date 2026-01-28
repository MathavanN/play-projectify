using System.Collections.Concurrent;

namespace Service.Product.Data;

public sealed class ProductStore
{
    // This is the single source of truth for all products
    public ConcurrentDictionary<int, Product> Store { get; }

    public ProductStore()
    {
        Store = new ConcurrentDictionary<int, Product>();

        // Seed initial data
        Store[1] = new Product(1, "PlayStation 5", "Prod-001");
        Store[2] = new Product(2, "Xbox Game", "Prod-002");
        Store[3] = new Product(3, "Canon EOS R50", "Prod-003");
        Store[4] = new Product(4, "Dell XPS 13", "Prod-004");
        Store[5] = new Product(5, "Dyson V12 Slim Cordless Vacuum", "Prod-005");
    }
}
