using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrismaPrimeInvest.Domain.Entities.Relationships;
using Microsoft.EntityFrameworkCore;

namespace PrismaPrimeInvest.Infra.Data.Configurations.Relationships;

public class UserFundConfiguration : BaseConfiguration<UserFund>
{
    public override void Configure(EntityTypeBuilder<UserFund> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Quantity).IsRequired();
        
        builder.HasOne(uf => uf.User)
                .WithMany(u => u.UserFunds)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uf => uf.Fund)
                .WithMany(f => f.UsersFund)
                .HasForeignKey(uf => uf.FundId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
