using PrismaPrimeInvest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrismaPrimeInvest.Infra.Data.Configurations;

public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.CreatedAt).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");;
        builder.Property(e => e.UpdatedAt).IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETUTCDATE()");;
    }
}
