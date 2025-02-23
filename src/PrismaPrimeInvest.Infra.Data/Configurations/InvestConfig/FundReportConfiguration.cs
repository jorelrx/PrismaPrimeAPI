using PrismaPrimeInvest.Domain.Entities.Invest;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrismaPrimeInvest.Infra.Data.Configurations.InvestConfig;

public class FundReportConfiguration : BaseConfiguration<FundReport>
{
    public override void Configure(EntityTypeBuilder<FundReport> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.ReportId).IsRequired();
        builder.Property(e => e.Type).IsRequired();
        builder.Property(e => e.ReferenceDate).IsRequired();
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.FundId).IsRequired();
        builder.HasOne(e => e.Fund).WithMany(e => e.Reports).HasForeignKey(e => e.FundId);
    }
}
