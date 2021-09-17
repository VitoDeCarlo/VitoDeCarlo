using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitoDeCarlo.Models.Identity;

namespace VitoDeCarlo.Data.Configuration.Identity;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UsersRoles", "id");
        builder.HasKey(r => new { r.UserId, r.RoleId });
    }
}
