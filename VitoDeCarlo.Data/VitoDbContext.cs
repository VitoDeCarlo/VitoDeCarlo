using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VitoDeCarlo.Data.Configuration;
using VitoDeCarlo.Data.Configuration.Identity;
using VitoDeCarlo.Data.Exceptions;
using VitoDeCarlo.Models.Geography;
using VitoDeCarlo.Models.Identity;

namespace VitoDeCarlo.Data;

public class VitoDbContext : DbContext
{
    public VitoDbContext(DbContextOptions<VitoDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserRole> UsersRoles { get; set; } = null!;
    public DbSet<UserClaim> UserClaims {  get; set; } = null!;
    public DbSet<RoleClaim> RoleClaims {  get; set; } = null!;
    public DbSet<UserLogin> UserLogins { get; set; } = null!;
    public DbSet<UserToken> UserTokens {  get; set; } = null!;

    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Region> Regions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new UserRoleConfiguration());
        builder.ApplyConfiguration(new UserClaimConfiguration());
        builder.ApplyConfiguration(new RoleClaimConfiguration());
        builder.ApplyConfiguration(new UserLoginConfiguration());
        builder.ApplyConfiguration(new UserTokenConfiguration());

        builder.ApplyConfiguration(new CountryConfiguration());
        builder.ApplyConfiguration(new RegionConfiguration());

        //builder.Entity<Country>().HasData(new Country[]
        //{
        //    new Country{ Id = "au", Code = "AU", Code3 = "AUS", Numeric = "036", Name = "Australia", OfficialName = "Australia", DialingCode = "61"},
        //    new Country{ Id = "ca", Code = "CA", Code3 = "CAN", Numeric = "124", Name = "Canada", OfficialName = "Canada", DialingCode = "1"},
        //    new Country{ Id = "mx", Code = "MX", Code3 = "MEX", Numeric = "484", Name = "Mexico", OfficialName = "The United Mexican States", DialingCode = "52"},
        //    new Country{ Id = "ro", Code = "RO", Code3 = "ROU", Numeric = "642", Name = "Romania", OfficialName = "Romania", DialingCode = "40"},
        //    new Country{ Id = "uk", Code = "GB", Code3 = "GBR", Numeric = "826", Name = "United Kingdom", OfficialName = "United Kingdom of Great Britain and Northern Ireland", DialingCode = "44"},
        //    new Country{ Id = "us", Code = "US", Code3 = "USA", Numeric = "840", Name = "United States", OfficialName = "United States of America", DialingCode = "1"},
        //});

    }

    public override int SaveChanges()
    {
        try
        {
            return base.SaveChanges();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // ToDo: Log and handle error
            throw new CustomConcurrencyException("Concurrency Exception: ", ex);
        }
        catch (RetryLimitExceededException ex)
        {
            // DbResiliency retry limit exceeded
            // ToDo: log and handle error
            throw new CustomRetryLimitExceededException("There is a problem with SQL Server.", ex);
        }
        catch (DbUpdateException ex)
        {
            // ToDo: log and handle error
            throw new CustomDbUpdateException("An error occurred updating the database.", ex);
        }
        catch (Exception ex)
        {
            // ToDo: log and handle error
            throw new CustomException("An error occurred updating the database.", ex);
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // ToDo: Log and handle error
            throw new CustomConcurrencyException("Concurrency Exception: ", ex);
        }
        catch (RetryLimitExceededException ex)
        {
            // DbResiliency retry limit exceeded
            // ToDo: log and handle error
            throw new CustomRetryLimitExceededException("There is a problem with SQL Server.", ex);
        }
        catch (DbUpdateException ex)
        {
            // ToDo: log and handle error
            throw new CustomDbUpdateException("An error occurred updating the database.", ex);
        }
        catch (Exception ex)
        {
            // ToDo: log and handle error
            throw new CustomException("An error occurred updating the database.", ex);
        }
    }
}
