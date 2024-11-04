using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrismaPrimeInvest.Domain.Entities.Relationships;
using Microsoft.EntityFrameworkCore;

namespace PrismaPrimeInvest.Infra.Data.Configurations.Relationships;

public class WalletFundConfiguration : BaseConfiguration<WalletFund>
{
    public override void Configure(EntityTypeBuilder<WalletFund> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(uf => uf.Wallet)
                .WithMany(u => u.WalletFunds)
                .HasForeignKey(uf => uf.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uf => uf.Fund)
                .WithMany(f => f.WalletFunds)
                .HasForeignKey(uf => uf.FundId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
