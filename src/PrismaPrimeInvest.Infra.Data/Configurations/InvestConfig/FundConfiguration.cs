using PrismaPrimeInvest.Domain.Entities.Invest;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrismaPrimeInvest.Infra.Data.Configurations.InvestConfig;

public class FundConfiguration : BaseConfiguration<Fund>
{
    public override void Configure(EntityTypeBuilder<Fund> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Code).IsRequired();
        builder.Property(e => e.Type).IsRequired();
    }
}
