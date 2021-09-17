using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitoDeCarlo.Models.Identity;

namespace VitoDeCarlo.Data.Configuration.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users", "id");

        b.HasKey(e => e.Id);

        // https://github.com/dotnet/aspnetcore/blob/main/src/Identity/EntityFrameworkCore/src/IdentityUserContext.cs
        //if (encryptPersonalData)
        //{
        //    converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
        //    var personalDataProps = typeof(TUser).GetProperties().Where(
        //                    prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
        //    foreach (var p in personalDataProps)
        //    {
        //        if (p.PropertyType != typeof(string))
        //        {
        //            throw new InvalidOperationException(Resources.CanOnlyProtectStrings);
        //        }
        //        b.Property(typeof(string), p.Name).HasConversion(converter);
        //    }
        //}


        b.Property(p => p.Id)
            .HasColumnType("bigint")
            .IsRequired();

        b.Property(e => e.UserName)
            .HasMaxLength(25)
            .HasColumnType("nvarchar(25)")
            .IsRequired();

        b.Property(e => e.NormalizedUserName)
            .HasMaxLength(25)
            .HasColumnType("nvarchar(25)")
            .IsRequired();

        b.Property(e => e.Email).HasMaxLength(256);
        b.Property(e => e.NormalizedEmail).HasMaxLength(256);

        b.Property(p => p.PasswordHash)
            .HasColumnType("nvarchar(200)")
            .IsRequired(false);

        b.Property(p => p.GivenName)
            .HasColumnType("nvarchar(50)");

        b.Property(p => p.FamilyName)
            .HasColumnType("nvarchar(50)");

        b.Property(p => p.Address1)
            .HasColumnType("nvarchar(100)");

        b.Property(p => p.Address2)
            .HasColumnType("nvarchar(100)");

        b.Property(p => p.CountryCode)
            .HasMaxLength(2)
            .HasColumnType("nchar(2)");

        b.Property(p => p.RegionCode)
            .HasMaxLength(3)
            .HasColumnType("nvarchar(3)");

        b.Property(p => p.City)
            .HasMaxLength(60)
            .HasColumnType("nvarchar(60)");

        b.Property(p => p.PostalCode)
            .HasMaxLength(10)
            .HasColumnType("nvarchar(10)");

        b.Property(p => p.BirthDate)
            .HasColumnType("date");

        b.Property(p => p.DialingCode)
            .HasMaxLength(3)
            .HasColumnType("nvarchar(3)");

        b.Property(p => p.PhoneNumber)
            .HasMaxLength(15)
            .HasColumnType("nvarchar(15)");

        b.Property(p => p.Gender)
            .HasMaxLength(1)
            .HasColumnType("nchar(1)");

        b.Property(p => p.RegisterDate)
            .HasColumnType("datetime2(0)")
            .IsRequired()
            .HasDefaultValue(DateTime.Now);

        b.Property(p => p.AuthenticationType)
            .HasColumnType("nvarchar(50)");

        b.Property(p => p.ConcurrencyStamp)
            .HasColumnType("nchar(36)")
            .IsRequired(false);

        b.Property(p => p.SecurityStamp)
            .HasColumnType("nchar(36)")
            .IsRequired(false);

        b.Property(e => e.AuthenticationType)
            .IsRequired(false);

        b.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();

        b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
        b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

        b.HasMany<UserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
        b.HasMany<UserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
        b.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
        b.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
    }
}
