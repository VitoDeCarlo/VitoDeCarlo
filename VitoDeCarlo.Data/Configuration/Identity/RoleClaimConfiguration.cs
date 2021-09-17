using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitoDeCarlo.Models.Identity;

namespace VitoDeCarlo.Data.Configuration.Identity;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable("RoleClaims", "id");
        builder.HasKey(pk => pk.Id);
        builder.Property(b => b.Id).HasColumnType("int");
        builder.Property(b => b.RoleId).HasColumnType("bigint");
        builder.Property(p => p.Type).HasColumnType("nvarchar(100)");
        builder.Property(p => p.Value).HasColumnType("nvarchar(1000)");
        builder.Property(p => p.ValueType).HasColumnType("nvarchar(100)");
        builder.Property(p => p.Issuer).HasColumnType("nvarchar(100)");
        builder.Property(p => p.OriginalIssuer).HasColumnType("nvarchar(100)");
    }
}
