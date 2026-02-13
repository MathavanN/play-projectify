using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayProjectify.ProductService.Models.Entites;

namespace PlayProjectify.ProductService.Data.Mappings;

internal class ProductMapping : BaseEntityMapping<Product>
{
    public override void Configure(EntityTypeBuilder<Product> entity)
    {
        // Configure base entity fields
        base.Configure(entity);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(e => e.Price)
            .IsRequired();

        entity.Property(e => e.StockQuantity)
            .IsRequired();

        entity.Property(e => e.CategoryId)
            .IsRequired();

        var electronicsId = new Guid("e4e9ef00-fc2b-4fde-8900-c3c69016ea18");
        var fashionId = new Guid("7e42e4ab-ea84-4055-8f73-f4cefd3998fe");
        var booksId = new Guid("d61d02f9-160f-439a-8f2d-d7dc87da4d99");
        var sportsId = new Guid("6cf79ab1-e24d-411f-a467-629807552051");
        var toysId = new Guid("c7f2295b-9ec3-4d52-9c0a-727350255749");

        var seedDate = new DateTime(2026, 2, 11, 13, 56, 0, DateTimeKind.Utc);

        entity.HasData(
            new Product
            {
                Id = new Guid("10000000-0000-0000-0000-000000000001"),
                Name = "Smartphone X12",
                Description = "6.5-inch display, 128GB storage, 5G support",
                Price = 799,
                StockQuantity = 30,
                CategoryId = electronicsId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("10000000-0000-0000-0000-000000000002"),
                Name = "Laptop Pro 15",
                Description = "15-inch laptop with 16GB RAM and 1TB SSD",
                Price = 1499,
                StockQuantity = 15,
                CategoryId = electronicsId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("10000000-0000-0000-0000-000000000003"),
                Name = "Bluetooth Headphones",
                Description = "Noise cancelling over-ear headphones",
                Price = 199,
                StockQuantity = 50,
                CategoryId = electronicsId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("10000000-0000-0000-0000-000000000004"),
                Name = "Smartwatch Active",
                Description = "Fitness tracking smartwatch with heart rate monitor",
                Price = 249,
                StockQuantity = 40,
                CategoryId = electronicsId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("10000000-0000-0000-0000-000000000005"),
                Name = "Portable Bluetooth Speaker",
                Description = "Waterproof speaker with deep bass",
                Price = 99,
                StockQuantity = 60,
                CategoryId = electronicsId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            new Product
            {
                Id = new Guid("20000000-0000-0000-0000-000000000001"),
                Name = "Remote Control Car",
                Description = "High-speed RC car with rechargeable battery",
                Price = 59,
                StockQuantity = 25,
                CategoryId = toysId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("20000000-0000-0000-0000-000000000002"),
                Name = "Building Blocks Set",
                Description = "500-piece creative building block set",
                Price = 49,
                StockQuantity = 40,
                CategoryId = toysId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("20000000-0000-0000-0000-000000000003"),
                Name = "Board Game: Strategy Wars",
                Description = "Multiplayer strategy board game for ages 10+",
                Price = 39,
                StockQuantity = 20,
                CategoryId = toysId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("20000000-0000-0000-0000-000000000004"),
                Name = "Puzzle 1000 Pieces",
                Description = "Scenic landscape puzzle for adults",
                Price = 29,
                StockQuantity = 35,
                CategoryId = toysId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("20000000-0000-0000-0000-000000000005"),
                Name = "Action Figure Set",
                Description = "Superhero action figure 5-pack",
                Price = 79,
                StockQuantity = 18,
                CategoryId = toysId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            new Product
            {
                Id = new Guid("30000000-0000-0000-0000-000000000001"),
                Name = "Treadmill Home Runner",
                Description = "Foldable treadmill with digital display",
                Price = 899,
                StockQuantity = 5,
                CategoryId = sportsId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            new Product
            {
                Id = new Guid("40000000-0000-0000-0000-000000000001"),
                Name = "Clean Code",
                Description = "A Handbook of Agile Software Craftsmanship",
                Price = 45,
                StockQuantity = 100,
                CategoryId = booksId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("40000000-0000-0000-0000-000000000002"),
                Name = "The Pragmatic Programmer",
                Description = "Your Journey to Mastery",
                Price = 50,
                StockQuantity = 80,
                CategoryId = booksId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            new Product
            {
                Id = new Guid("50000000-0000-0000-0000-000000000001"),
                Name = "Men's Casual T-Shirt",
                Description = "100% cotton, breathable fabric",
                Price = 19,
                StockQuantity = 120,
                CategoryId = fashionId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("50000000-0000-0000-0000-000000000002"),
                Name = "Women's Denim Jacket",
                Description = "Classic fit blue denim jacket",
                Price = 79,
                StockQuantity = 45,
                CategoryId = fashionId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("50000000-0000-0000-0000-000000000003"),
                Name = "Running Sneakers",
                Description = "Lightweight shoes for everyday training",
                Price = 99,
                StockQuantity = 60,
                CategoryId = fashionId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = new Guid("50000000-0000-0000-0000-000000000004"),
                Name = "Leather Wallet",
                Description = "Genuine leather slim wallet",
                Price = 35,
                StockQuantity = 75,
                CategoryId = fashionId,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            }
        );
    }
}
