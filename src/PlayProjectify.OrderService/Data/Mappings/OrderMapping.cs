using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayProjectify.OrderService.Models.Entites;

namespace PlayProjectify.OrderService.Data.Mappings;

internal class OrderMapping : BaseEntityMapping<Order>
{
    public override void Configure(EntityTypeBuilder<Order> entity)
    {
        // Call base mapping first
        base.Configure(entity);

        entity.Property(e => e.TotalAmount)
            .IsRequired();

        entity.Property(e => e.CustomerId)
            .IsRequired();

        entity.Property(e => e.OrderStatus)
            .IsRequired();

        entity.HasMany(e => e.Items) //A category can have many Products
            .WithOne(p => p.Order) //each product has one category
            .HasForeignKey(p => p.OrderId) //the foreign key in Product table
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid deleting products when a category is deleted

    }
}
