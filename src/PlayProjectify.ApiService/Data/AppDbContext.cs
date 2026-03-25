using Microsoft.EntityFrameworkCore;
using PlayProjectify.ApiService.Models.Entites;

namespace PlayProjectify.ApiService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Address> Address => Set<Address>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.FirstName).IsRequired();
            entity.Property(o => o.LastName).IsRequired();
            entity.Property(o => o.Email).IsRequired();
            entity.Property(o => o.PhoneNumber).IsRequired();
            entity.Property(o => o.OrderStatus).IsRequired();

            entity.Property(o => o.TotalAmount)
                  .HasColumnType("decimal(18,2)");

            // Shipping Address (Required)
            entity.HasOne(o => o.ShippingAddress)
                  .WithMany()
                  .HasForeignKey(o => o.ShippingAddressId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Restrict);

            // Billing Address (Optional)
            entity.HasOne(o => o.BillingAddress)
                  .WithMany()
                  .HasForeignKey(o => o.BillingAddressId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Order -> OrderItems (1-to-many)
            entity.HasMany(o => o.Items)
                  .WithOne(i => i.Order)
                  .HasForeignKey(i => i.OrderId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Street1).IsRequired();
            entity.Property(a => a.City).IsRequired();
            entity.Property(a => a.State).IsRequired();
            entity.Property(a => a.PostalCode).IsRequired();
            entity.Property(a => a.Country).IsRequired();
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.ProductName).IsRequired();

            entity.Property(i => i.UnitPrice)
                  .HasColumnType("decimal(18,2)");

            // Computed in C#, not stored
            entity.Ignore(i => i.TotalPrice);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.FirstName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(c => c.LastName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(c => c.Email)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.HasIndex(c => c.Email)
                  .IsUnique();

            entity.Property(c => c.PhoneNumber)
                  .HasMaxLength(30);

            entity.Property(c => c.CompanyName)
                  .HasMaxLength(200);

            entity.Property(c => c.Notes)
                  .HasMaxLength(1000);

            entity.Property(c => c.PreferredCurrency)
                  .IsRequired()
                  .HasMaxLength(3)       // ISO 4217: CHF, EUR, USD
                  .HasDefaultValue("CHF");

            entity.Property(c => c.PreferredLanguage)
                  .IsRequired()
                  .HasMaxLength(10)      // BCP 47: en, de, fr, ta
                  .HasDefaultValue("en");

            entity.Property(c => c.MarketingOptIn)
                  .IsRequired()
                  .HasDefaultValue(false);

            entity.Property(c => c.IsActive)
                  .IsRequired()
                  .HasDefaultValue(true);
        });
    }
}
