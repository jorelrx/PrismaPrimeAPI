using PrismaPrimeInvest.Domain.Entities.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PrismaPrimeInvest.Infra.Data.Configurations.UserConfig;

public class WalletConfiguration : BaseConfiguration<Wallet>
{
    public override void Configure(EntityTypeBuilder<Wallet> builder)
    {
        base.Configure(builder);
        builder.ToTable("Wallet");
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.CreatedByUserId).IsRequired();
        builder.Property(e => e.IsPublic).IsRequired();
        builder.Property(e => e.WalletType).IsRequired();
        
        builder.HasOne(e => e.CreatedByUser)
            .WithMany()
            .HasForeignKey(w => w.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
