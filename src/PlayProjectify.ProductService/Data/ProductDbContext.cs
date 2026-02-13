using Microsoft.EntityFrameworkCore;
using PlayProjectify.ProductService.Data.Mappings;
using PlayProjectify.ProductService.Models.Entites;

namespace PlayProjectify.ProductService.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CategoryMapping());
        modelBuilder.ApplyConfiguration(new ProductMapping());
    }
}