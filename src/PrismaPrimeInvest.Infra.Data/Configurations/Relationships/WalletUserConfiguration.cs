using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrismaPrimeInvest.Domain.Entities.Relationships;
using Microsoft.EntityFrameworkCore;

namespace PrismaPrimeInvest.Infra.Data.Configurations.Relationships;

public class WalletUserConfiguration : BaseConfiguration<WalletUser>
{
    public override void Configure(EntityTypeBuilder<WalletUser> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(uf => uf.Wallet)
                .WithMany(u => u.WalletUsers)
                .HasForeignKey(uf => uf.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uf => uf.User)
                .WithMany(f => f.WalletUsers)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
