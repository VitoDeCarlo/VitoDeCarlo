using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using VitoDeCarlo.Models.Identity;

namespace VitoDeCarlo.Data.Identity;

public class RoleStore : IRoleStore<Role>,
    IRoleClaimStore<Role>,
    IQueryableRoleStore<Role>
{
    private readonly IDbContextFactory<VitoDbContext> contextFactory;
    private readonly ILogger _logger;

    public RoleStore(IDbContextFactory<VitoDbContext> contextFactory, ILogger<RoleStore> logger)
    {
        this.contextFactory = contextFactory;
        _logger = logger;
    }


    /// <summary>
    /// Asynchronously Create a Role
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("RoleStore.CreateAsync called for role {name}", role.Name);
        if (role == null) throw new ArgumentNullException(nameof(role));
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        using var context = contextFactory.CreateDbContext();
        context.Add(role);
        var x = await context.SaveChangesAsync(cancellationToken);
        if (x > 0)
            return IdentityResult.Success;
        return IdentityResult.Failed(new IdentityError { Description = $"Could not insert role {role.Name}." });
    }


    /// <summary>
    /// Asynchronously Update a Role
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RoleStore.UpdateAsync called for role {name}", role.Name);
        if (role == null) throw new ArgumentNullException(nameof(role));
        cancellationToken.ThrowIfCancellationRequested();

        using var context = contextFactory.CreateDbContext();
        context.Update<Role>(role);
        var x = await context.SaveChangesAsync(cancellationToken);
        if (x > 0)
            return IdentityResult.Success;
        return IdentityResult.Failed(new IdentityError { Description = $"Could not update role {role.Name}." });
    }


    /// <summary>
    /// Asynchronously Delete a Role
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RoleStore.DeleteAsync called for role {name}", role.Name);
        if (role == null) throw new ArgumentNullException(nameof(role));
        cancellationToken.ThrowIfCancellationRequested();

        using var context = contextFactory.CreateDbContext();
        context.Remove<Role>(role);
        var x = await context.SaveChangesAsync(cancellationToken);
        if (x > 0)
            return IdentityResult.Success;
        return IdentityResult.Failed(new IdentityError { Description = $"Could not delete role {role.Name}." });
    }


    #region Lookup Role
    /// <summary>
    /// Asynchronously Lookup a Role by Id
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RoleStore.FindByIdAsync called for role {roleId}", roleId);
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrWhiteSpace(roleId))
        {
            throw new ArgumentNullException(nameof(roleId));
        }

        if (!long.TryParse(roleId, out long id))
        {
            throw new ArgumentException("Not a valid long UserId", nameof(roleId));
        }

        using var context = contextFactory.CreateDbContext();
        var role = await context.Roles.FindAsync(new object[] { id }, cancellationToken);
        return role!;
    }

    /// <summary>
    /// Asynchonously Lookup a Role by Name
    /// </summary>
    /// <param name="normalizedRoleName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Role> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RoleStore.FindByNameAsync called for role {name}", name);
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }
        using var context = contextFactory.CreateDbContext();
        var role = await context.Roles.SingleOrDefaultAsync(r => r.Name.Equals(name), cancellationToken);
        return role!;
    }
    #endregion


    #region Get Property of Role
    /// <summary>
    /// Get Id
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RoleStore.GetRoleIdAsync called for role {name}", role.Name);
        cancellationToken.ThrowIfCancellationRequested();
        if (role == null) throw new ArgumentNullException(nameof(role));
        return Task.FromResult(role.Id.ToString());
    }

    /// <summary>
    /// Asynchronously Lookup a PulseRole's Name
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RoleStore.GetRoleNameAsync called for role {name}", role.Name);
        cancellationToken.ThrowIfCancellationRequested();
        if (role == null) throw new ArgumentNullException(nameof(role));
        return Task.FromResult(role.Name);
    }

    /// <summary>
    /// Get NormalizedRoleName
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RoleStore.GetNormalizedRoleNameAsync called for role {name}", role.Name);
        cancellationToken.ThrowIfCancellationRequested();
        if (role == null) throw new ArgumentNullException(nameof(role));
        return Task.FromResult(role.NormalizedName);
    }
    #endregion


    #region Set Property of Role
    public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RoleStore.SetRoleNameAsync called for role {name}", role.Name);
        cancellationToken.ThrowIfCancellationRequested();
        if (role == null) throw new ArgumentNullException(nameof(role));
        if (string.IsNullOrWhiteSpace(roleName)) throw new ArgumentNullException(nameof(roleName));
        role.Name = roleName;
        return Task.CompletedTask;
    }

    public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RoleStore.SetNormalizedRoleNameAsync called for role {name}", role.Name);
        cancellationToken.ThrowIfCancellationRequested();
        if (role == null) throw new ArgumentNullException(nameof(role));
        if (string.IsNullOrWhiteSpace(normalizedName)) throw new ArgumentNullException(nameof(normalizedName));
        role.NormalizedName = normalizedName;
        return Task.CompletedTask;
    }
    #endregion


    #region IQueryableRoleStore
    public IQueryable<Role> Roles
    {
        get
        {
            _logger.LogInformation("RoleStore.IQueryable called for roles");
            using var context = contextFactory.CreateDbContext();
            return context.Set<Role>();
        }
    }
    #endregion

    #region IRoleClaimStore
    public Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("RoleStore.AddClaimsAsync called for role {name}", role.Name);
        if (role == null) throw new ArgumentNullException(nameof(role));
        if (claim == null) throw new ArgumentNullException(nameof(claim));
        cancellationToken.ThrowIfCancellationRequested();
        RoleClaim roleClaim = new(role.Id, claim);
        using var context = contextFactory.CreateDbContext();
        context.Update<RoleClaim>(roleClaim);
        return Task.FromResult(context.SaveChanges());
    }

    public Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("RoleStore.GetClaimsAsync called for role {name}", role.Name);
        if (role == null) throw new ArgumentNullException(nameof(role));
        cancellationToken.ThrowIfCancellationRequested();
        IList<Claim> claims = new List<Claim>();
        using var context = contextFactory.CreateDbContext();
        claims = context.RoleClaims
            .Where(rc => rc.RoleId.Equals(role.Id))
            .Select(c => c.ToClaim()).ToList();
        return Task.FromResult(claims);
    }

    public Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("RoleStore.RemoveClaimAsync called for claim {subject}", claim.Subject);
        if (role == null) throw new ArgumentNullException(nameof(role));
        if (claim == null) throw new ArgumentNullException(nameof(claim));
        cancellationToken.ThrowIfCancellationRequested();
        RoleClaim rc = new(role.Id, claim);
        using var context = contextFactory.CreateDbContext();
        context.RoleClaims.Remove(rc);
        return Task.FromResult(context.SaveChanges());
    }
    #endregion

    private bool _disposed;

    /// <summary>
    /// Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }

    /// <summary>
    /// Dispose the stores
    /// </summary>
    public void Dispose()
    {
        _disposed = true;
    }
}
