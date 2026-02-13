namespace PlayProjectify.ProductService.Models.Entites;

public class Product : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    // Category relation (many-to-1)
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}