using PrismaPrimeInvest.Domain.Entities.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrismaPrimeInvest.Infra.Data.Configurations.UserConfig;

public class UserConfiguration : BaseConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.FirstName).IsRequired();
        builder.Property(e => e.LastName).IsRequired();
        builder.Property(e => e.Document).IsRequired();
    }
}