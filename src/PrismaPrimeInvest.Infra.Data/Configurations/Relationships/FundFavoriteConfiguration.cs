using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrismaPrimeInvest.Domain.Entities.Relationships;

namespace PrismaPrimeInvest.Infra.Data.Configurations.Relationships;

public class FundFavoriteConfiguration : BaseConfiguration<FundFavorite>
{
    public override void Configure(EntityTypeBuilder<FundFavorite> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(ff => ff.Id);

        builder.HasOne(ff => ff.User)
               .WithMany(u => u.FavoriteFunds)
               .HasForeignKey(ff => ff.UserId);

        builder.HasOne(ff => ff.Fund)
               .WithMany()
               .HasForeignKey(ff => ff.FundId);
    }
}
