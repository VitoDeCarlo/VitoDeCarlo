using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitoDeCarlo.Models.Geography;

namespace VitoDeCarlo.Data.Configuration;

public class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.ToTable("Regions", "geo");
        builder.HasKey(e => e.Id);


        builder.Property(e => e.Id)
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.Name)
            .HasMaxLength(35)
            .IsRequired();

        builder.Property(e => e.CountryId)
            .HasMaxLength(2)
            .IsRequired();
    }
}
