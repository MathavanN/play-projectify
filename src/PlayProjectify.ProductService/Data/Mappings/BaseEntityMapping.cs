using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayProjectify.ProductService.Models.Entites;

namespace PlayProjectify.ProductService.Data.Mappings;

internal class BaseEntityMapping<TBase> : IEntityTypeConfiguration<TBase> where TBase : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TBase> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.CreatedAt)
            .IsRequired();
        entity.Property(e => e.UpdatedAt)
            .IsRequired();
    }
}
