using PrismaPrimeInvest.Domain.Entities.Invest;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrismaPrimeInvest.Infra.Data.Configurations.InvestConfig;

public class FundConfiguration : BaseConfiguration<Fund>
{
    public override void Configure(EntityTypeBuilder<Fund> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Cnpj).IsRequired();
        builder.Property(e => e.Code).IsRequired();
        builder.Property(e => e.QtyQuotasIssued).IsRequired();
        builder.Property(e => e.NetAssetValue).IsRequired();
        builder.Property(e => e.TotalShareholders).IsRequired();
        builder.Property(e => e.NetAssetValuePerShare).IsRequired();
        builder.Property(e => e.DividendYield).IsRequired();
        builder.Property(e => e.Price).IsRequired();
        builder.Property(e => e.MaxPrice).IsRequired();
        builder.Property(e => e.MinPrice).IsRequired();
        builder.Property(e => e.BestBuyDay).IsRequired();
        builder.Property(e => e.BestBuyDayPrice).IsRequired();
        builder.Property(e => e.Type).IsRequired();
    }
}
