using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitoDeCarlo.Models.Geography;

namespace VitoDeCarlo.Data.Configuration;
public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries", "geo");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(e => e.Code3)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.Numeric)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.Name)
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(e => e.OfficialName)
            .HasMaxLength(75)
            .IsRequired();

        builder.Property(e => e.DialingCode)
            .HasMaxLength(3)
            .IsRequired();

        builder.HasMany(e => e.Regions).WithOne(r => r.Country).HasForeignKey(r => r.CountryId);
    }
}
