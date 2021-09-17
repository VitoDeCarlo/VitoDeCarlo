using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitoDeCarlo.Models.Identity;

namespace VitoDeCarlo.Data.Configuration.Identity;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable("UserLogins", "id");
        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });

        builder.Property(p => p.LoginProvider).HasMaxLength(256);
        builder.Property(p => p.ProviderKey).HasMaxLength(256);
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.ProviderDisplayName).HasMaxLength(256).IsRequired(false);
        builder.Property(e => e.LoginTime).IsRequired();
    }
}
