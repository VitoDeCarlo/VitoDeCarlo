using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitoDeCarlo.Models.Identity;

namespace VitoDeCarlo.Data.Configuration.Identity;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable("UserTokens", "id");
        builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

        builder.Property(t => t.UserId).HasColumnType("bigint");
        builder.Property(t => t.LoginProvider).HasMaxLength(100);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(p => p.Value).HasMaxLength(2000).IsRequired(false);
    }
}
