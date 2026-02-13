using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayProjectify.OrderService.Models.Entites;

namespace PlayProjectify.OrderService.Data.Mappings;

internal class OrderItemMapping : BaseEntityMapping<OrderItem>
{
    public override void Configure(EntityTypeBuilder<OrderItem> entity)
    {
        // Call base mapping first
        base.Configure(entity);

        entity.Property(e => e.OrderId)
            .IsRequired();

        entity.Property(e => e.ProductId)
            .IsRequired();

        entity.Property(e => e.ProductName)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(e => e.UnitPrice)
            .IsRequired();

        entity.Property(e => e.Quantity)
            .IsRequired();
    }
}