namespace PlayProjectify.ProductService.Models.Entites;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    // Navigation property
    public ICollection<Product> Products { get; set; } = [];
}
