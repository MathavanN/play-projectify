using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayProjectify.ProductService.Models.Entites;

namespace PlayProjectify.ProductService.Data.Mappings;

internal class CategoryMapping : BaseEntityMapping<Category>
{
    public override void Configure(EntityTypeBuilder<Category> entity)
    {
        // Call base mapping first
        base.Configure(entity);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        entity.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(250);

        entity.HasMany(e => e.Products) //A category can have many Products
            .WithOne(p => p.Category) //each product has one category
            .HasForeignKey(p => p.CategoryId) //the foreign key in Product table
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid deleting products when a category is deleted

        var seedDate = new DateTime(2026, 2, 11, 13, 49, 0, DateTimeKind.Utc);
        entity.HasData(
            new Category
            {
                Id = new Guid("e4e9ef00-fc2b-4fde-8900-c3c69016ea18"),
                Name = "Electronics",
                Description = "Electronic devices and gadgets.",
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Category
            {
                Id = new Guid("7e42e4ab-ea84-4055-8f73-f4cefd3998fe"),
                Name = "Fashion",
                Description = "Clothing, shoes, and accessories for men and women.",
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Category
            {
                Id = new Guid("d61d02f9-160f-439a-8f2d-d7dc87da4d99"),
                Name = "Books",
                Description = "Printed and digital books across all genres.",
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Category
            {
                Id = new Guid("6cf79ab1-e24d-411f-a467-629807552051"),
                Name = "Sports & Outdoors",
                Description = "Fitness equipment, outdoor gear, and sports accessories.",
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Category
            {
                Id = new Guid("c7f2295b-9ec3-4d52-9c0a-727350255749"),
                Name = "Toys & Games",
                Description = "Children’s toys, board games, and entertainment items.",
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            });
    }
}
