using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using VitoDeCarlo.Models.Identity;

namespace VitoDeCarlo.Data.Identity;

public class UserStore : 
    IUserStore<User>,
    IUserPasswordStore<User>,
    IUserEmailStore<User>,
    IUserPhoneNumberStore<User>,
    IUserConfirmation<User>,
    IQueryableUserStore<User>,
    IUserLoginStore<User>,
    IUserClaimStore<User>,
    IUserRoleStore<User>,
    IUserAuthenticationTokenStore<User>,
    IUserAuthenticatorKeyStore<User>,
    IUserTwoFactorStore<User>,
    IUserTwoFactorRecoveryCodeStore<User>
{
    private readonly IDbContextFactory<VitoDbContext> contextFactory;
    private readonly ILogger _logger;
    private readonly CancellationTokenSource _cancellationTokenSource = new();


    public UserStore(IDbContextFactory<VitoDbContext> contextFactory, ILogger<UserStore> logger)
    {
        this.contextFactory = contextFactory;
        _logger = logger;
    }

    #region IUserStore
    /// <summary>
    /// Gets the user identifier for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose identifier should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user"/>.</returns>
    Task<string> IUserStore<User>.GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.GetUserIdAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return Task.FromResult(user.Id.ToString());
    }

    /// <summary>
    /// Gets the user name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose name should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the name for the specified <paramref name="user"/>.</returns>
    Task<string> IUserStore<User>.GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.GetUserNameAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.UserName);
    }

    /// <summary>
    /// Sets the given <paramref name="userName" /> for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose name should be set.</param>
    /// <param name="userName">The user name to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task IUserStore<User>.SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.SetUserNameAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.UserName = userName;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the normalized user name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose normalized name should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the normalized user name for the specified <paramref name="user"/>.</returns>
    Task<string> IUserStore<User>.GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.GetNormalizedUserNameAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.NormalizedUserName);
    }

    /// <summary>
    /// Sets the given normalized name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose name should be set.</param>
    /// <param name="normalizedName">The normalized name to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task IUserStore<User>.SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.SetNormalizedUserNameAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Creates the specified <paramref name="user"/> in the user store.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the creation operation.</returns>
    async Task<IdentityResult> IUserStore<User>.CreateAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.CreateAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        using var context = contextFactory.CreateDbContext();
        context.Add(user);
        await context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    /// <summary>
    /// Updates the specified <paramref name="user"/> in the user store.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
    async Task<IdentityResult> IUserStore<User>.UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.UpdateAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        using var context = contextFactory.CreateDbContext();
        context.Attach(user);
        user.ConcurrencyStamp = Guid.NewGuid().ToString();
        context.Update(user);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            var describer = new IdentityErrorDescriber();
            return IdentityResult.Failed(describer.ConcurrencyFailure());
        }
        return IdentityResult.Success;
    }

    /// <summary>
    /// Deletes the specified <paramref name="user"/> from the user store.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
    async Task<IdentityResult> IUserStore<User>.DeleteAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.DeleteAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        using var context = contextFactory.CreateDbContext();
        context.Remove(user);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            var describer = new IdentityErrorDescriber();
            return IdentityResult.Failed(describer.ConcurrencyFailure());
        }
        return IdentityResult.Success;
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">The user ID to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId"/> if it exists.
    /// </returns>
    async Task<User> IUserStore<User>.FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.FindByIdAsync called for user {userId}", userId);
        cancellationToken.ThrowIfCancellationRequested();
        if (userId is null)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        if (!long.TryParse(userId, out long id))
        {
            throw new ArgumentException("Not a valid long UserId", nameof(userId));
        }

        using var context = contextFactory.CreateDbContext();
        var user = await context.Users.FindAsync(new object[] { id }, cancellationToken);
        return user!;
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified normalized user name.
    /// </summary>
    /// <param name="normalizedUserName">The normalized user name to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="normalizedUserName"/> if it exists.
    /// </returns>
    async Task<User> IUserStore<User>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.FindByNameAsync called for user {normalizedUserName}", normalizedUserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (normalizedUserName is null)
        {
            throw new ArgumentNullException(nameof(normalizedUserName));
        }

        using var context = contextFactory.CreateDbContext();
        var user = await context.Users.SingleOrDefaultAsync(u => u.NormalizedUserName.Equals(normalizedUserName), cancellationToken);
        return user!;
    }
    #endregion

    #region IUserPasswordStore
    /// <summary>
    /// Sets the password hash for a user.
    /// </summary>
    /// <param name="user">The user to set the password hash for.</param>
    /// <param name="passwordHash">The password hash to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task IUserPasswordStore<User>.SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.SetPasswordHashAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the password hash for a user.
    /// </summary>
    /// <param name="user">The user to retrieve the password hash for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the password hash for the user.</returns>
    Task<string> IUserPasswordStore<User>.GetPasswordHashAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.GetPasswordHashAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.PasswordHash);
    }

    /// <summary>
    /// Returns a flag indicating if the specified user has a password.
    /// </summary>
    /// <param name="user">The user to retrieve the password hash for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> containing a flag indicating if the specified user has a password. If the
    /// user has a password the returned value with be true, otherwise it will be false.</returns>
    Task<bool> IUserPasswordStore<User>.HasPasswordAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("HasPasswordAsync.UpdateAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.PasswordHash is not null);
    }
    #endregion

    #region IUserEmailStore
    /// <summary>
    /// Sets the <paramref name="email"/> address for a <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose email should be set.</param>
    /// <param name="email">The email to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task IUserEmailStore<User>.SetEmailAsync(User user, string email, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.SetEmailAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.Email = email;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the email address for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose email should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The task object containing the results of the asynchronous operation, the email address for the specified <paramref name="user"/>.</returns>
    Task<string> IUserEmailStore<User>.GetEmailAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.GetEmailAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.Email);
    }

    /// <summary>
    /// Gets a flag indicating whether the email address for the specified <paramref name="user"/> has been verified, true if the email address is verified otherwise
    /// false.
    /// </summary>
    /// <param name="user">The user whose email confirmation status should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The task object containing the results of the asynchronous operation, a flag indicating whether the email address for the specified <paramref name="user"/>
    /// has been confirmed or not.
    /// </returns>
    Task<bool> IUserEmailStore<User>.GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.GetEmailConfirmedAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.EmailConfirmed);
    }

    /// <summary>
    /// Sets the flag indicating whether the specified <paramref name="user"/>'s email address has been confirmed or not.
    /// </summary>
    /// <param name="user">The user whose email confirmation status should be set.</param>
    /// <param name="confirmed">A flag indicating if the email address has been confirmed, true if the address is confirmed otherwise false.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task IUserEmailStore<User>.SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.SetEmailConfirmedAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the user, if any, associated with the specified, normalized email address.
    /// </summary>
    /// <param name="normalizedEmail">The normalized email address to return the user for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified normalized email address.
    /// </returns>
    Task<User> IUserEmailStore<User>.FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.FindByEmailAsync called for user {normalizedEmail}", normalizedEmail);
        cancellationToken.ThrowIfCancellationRequested();
        if (normalizedEmail is null)
        {
            throw new ArgumentNullException(nameof(normalizedEmail));
        }
        using var context = contextFactory.CreateDbContext();
        return context.Users.SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken)!;
    }

    /// <summary>
    /// Returns the normalized email for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose email address to retrieve.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The task object containing the results of the asynchronous lookup operation, the normalized email address if any associated with the specified user.
    /// </returns>
    Task<string> IUserEmailStore<User>.GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.GetNormalizedEmailAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.NormalizedEmail);
    }

    /// <summary>
    /// Sets the normalized email for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose email address to set.</param>
    /// <param name="normalizedEmail">The normalized email to set for the specified <paramref name="user"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task IUserEmailStore<User>.SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.SetNormalizedEmailAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }
    #endregion

    #region IUserPhoneNumberStore
    /// <summary>
    /// Sets the telephone number for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose telephone number should be set.</param>
    /// <param name="phoneNumber">The telephone number to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task IUserPhoneNumberStore<User>.SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.SetPhoneNumberConfirmedAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.PhoneNumber = phoneNumber;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the telephone number, if any, for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose telephone number should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the user's telephone number, if any.</returns>
    Task<string> IUserPhoneNumberStore<User>.GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.GetPhoneNumberAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.PhoneNumber)!;
    }

    /// <summary>
    /// Gets a flag indicating whether the specified <paramref name="user"/>'s telephone number has been confirmed.
    /// </summary>
    /// <param name="user">The user to return a flag for, indicating whether their telephone number is confirmed.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, returning true if the specified <paramref name="user"/> has a confirmed
    /// telephone number otherwise false.
    /// </returns>
    Task<bool> IUserPhoneNumberStore<User>.GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.GetPhoneNumberConfirmedAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.PhoneNumberConfirmed);
    }

    /// <summary>
    /// Sets a flag indicating if the specified <paramref name="user"/>'s phone number has been confirmed..
    /// </summary>
    /// <param name="user">The user whose telephone number confirmation status should be set.</param>
    /// <param name="confirmed">A flag indicating whether the user's telephone number has been confirmed.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task IUserPhoneNumberStore<User>.SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.SetPhoneNumberConfirmedAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.PhoneNumberConfirmed = confirmed;
        return Task.CompletedTask;
    }
    #endregion

    #region IUserConfirmation
    /// <summary>
    /// Determines whether the specified <paramref name="user"/> is confirmed.
    /// </summary>
    /// <param name="manager">The <see cref="UserManager{TUser}"/> that can be used to retrieve user properties.</param>
    /// <param name="user">The user.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the confirmation operation.</returns>
    Task<bool> IUserConfirmation<User>.IsConfirmedAsync(UserManager<User> manager, User user)
    {
        _logger.LogInformation("UserStore.IsConfirmedAsync called for user {userName}", user.UserName);
        if (manager is null)
        {
            throw new ArgumentNullException(nameof(manager));
        }
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return manager.IsEmailConfirmedAsync(user);
    }
    #endregion

    #region IUserLoginStore
    /// <summary>
    /// Adds the <paramref name="login"/> given to the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to add the login to.</param>
    /// <param name="login">The login to add to the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task IUserLoginStore<User>.AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserLoginStore.AddLoginAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (login is null)
        {
            throw new ArgumentNullException(nameof(login));
        }
        UserLogin userLogin = new() { 
            UserId = user.Id, 
            LoginProvider = login.LoginProvider, 
            ProviderDisplayName = login.ProviderDisplayName, 
            ProviderKey = login.ProviderKey, 
            LoginTime = DateTime.UtcNow
        };
        using var context = contextFactory.CreateDbContext();
        context.UserLogins.Add(userLogin);
        return Task.FromResult(context.SaveChanges());
    }

    /// <summary>
    /// Removes the <paramref name="loginProvider"/> given from the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to remove the login from.</param>
    /// <param name="loginProvider">The login to remove from the user.</param>
    /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    async Task IUserLoginStore<User>.RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserLoginStore.RemoveLoginAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        using var context = contextFactory.CreateDbContext();
        //var login = await context.UserLogins.SingleOrDefaultAsync(ul => ul.UserId.Equals(user.Id) && ul.LoginProvider.Equals(loginProvider) && ul.ProviderKey.Equals(providerKey), cancellationToken);
        var login = await context.UserLogins.FindAsync(new object[] { loginProvider, providerKey }, cancellationToken: cancellationToken);
        if (login is not null && login.UserId.Equals(user.Id))
        {
            context.UserLogins.Remove(login);
        }
    }

    /// <summary>
    /// Retrieves the associated logins for the specified <param ref="user"/>.
    /// </summary>
    /// <param name="user">The user whose associated logins to retrieve.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> for the asynchronous operation, containing a list of <see cref="UserLoginInfo"/> for the specified <paramref name="user"/>, if any.
    /// </returns>
    async Task<IList<UserLoginInfo>> IUserLoginStore<User>.GetLoginsAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserLoginStore.GetLoginsAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var userId = user.Id;
        using var context = contextFactory.CreateDbContext();
        return await context.UserLogins.Where(l => l.UserId.Equals(userId))
            .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the user associated with the specified login provider and login provider key.
    /// </summary>
    /// <param name="loginProvider">The login provider who provided the <paramref name="providerKey"/>.</param>
    /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> for the asynchronous operation, containing the user, if any which matched the specified login provider and key.
    /// </returns>
    async Task<User> IUserLoginStore<User>.FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserLoginStore.FindByLoginAsync called for loginProvider {loginProvider}", loginProvider);
        cancellationToken.ThrowIfCancellationRequested();

        using var context = contextFactory.CreateDbContext();
        var login = await context.UserLogins.FindAsync(new object[] { loginProvider, providerKey }, cancellationToken: cancellationToken);
        if (login is not null)
        {
            var user = await context.Users.FindAsync(new object[] { login.UserId }, cancellationToken: cancellationToken);
            return user!;
        }
        return null!;
    }
    #endregion

    #region IUserRoleStore
    /// <summary>
    /// Adds the given <paramref name="normalizedRoleName"/> to the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to add the role to.</param>
    /// <param name="normalizedRoleName">The role to add.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    async Task IUserRoleStore<User>.AddToRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.AddToRoleAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (string.IsNullOrWhiteSpace(normalizedRoleName))
        {
            throw new ArgumentNullException(nameof(normalizedRoleName));
        }
        using var context = contextFactory.CreateDbContext();
        var role = await context.Roles.SingleOrDefaultAsync(r => r.NormalizedName.Equals(normalizedRoleName), cancellationToken: cancellationToken);
        if (role is null)
        {
            throw new InvalidOperationException($"Role not found for {normalizedRoleName}");
        }
        context.UsersRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
    }

    /// <summary>
    /// Removes the given <paramref name="normalizedRoleName"/> from the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to remove the role from.</param>
    /// <param name="normalizedRoleName">The role to remove.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    async Task IUserRoleStore<User>.RemoveFromRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.RemoveFromRoleAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (string.IsNullOrWhiteSpace(normalizedRoleName))
        {
            throw new ArgumentNullException(nameof(normalizedRoleName));
        }
        using var context = contextFactory.CreateDbContext();
        var role = await context.Roles.SingleOrDefaultAsync(r => r.NormalizedName.Equals(normalizedRoleName), cancellationToken: cancellationToken);
        if (role is not null)
        {
            var userRole = await context.UsersRoles.FindAsync(new object[] { user.Id, role.Id }, cancellationToken);
            if (userRole is not null)
            {
                context.UsersRoles.Remove(userRole);
            }
        }
    }

    /// <summary>
    /// Retrieves the roles the specified <paramref name="user"/> is a member of.
    /// </summary>
    /// <param name="user">The user whose roles should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the roles the user is a member of.</returns>
    async Task<IList<string>> IUserRoleStore<User>.GetRolesAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.GetRolesAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        using var context = contextFactory.CreateDbContext();
        var userId = user.Id;
        var query = from userRole in context.UsersRoles
                    join role in context.Roles on userRole.RoleId equals role.Id
                    where userRole.UserId.Equals(userId)
                    select role.Name;
        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns a flag indicating if the specified user is a member of the give <paramref name="normalizedRoleName"/>.
    /// </summary>
    /// <param name="user">The user whose role membership should be checked.</param>
    /// <param name="normalizedRoleName">The role to check membership of</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> containing a flag indicating if the specified user is a member of the given group. If the
    /// user is a member of the group the returned value with be true, otherwise it will be false.</returns>
    async Task<bool> IUserRoleStore<User>.IsInRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.IsInRoleAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (string.IsNullOrWhiteSpace(normalizedRoleName))
        {
            throw new ArgumentNullException(nameof(normalizedRoleName));
        }
        using var context = contextFactory.CreateDbContext();
        var role = await context.Roles.SingleOrDefaultAsync(r => r.NormalizedName.Equals(normalizedRoleName), cancellationToken: cancellationToken);
        if (role is not null)
        {
            var userRole = await context.UsersRoles.FindAsync(new object[] { user.Id, role.Id }, cancellationToken);
            return userRole is not null;
        }
        return false;
    }

    /// <summary>
    /// Retrieves all users in the specified role.
    /// </summary>
    /// <param name="normalizedRoleName">The role whose users should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> contains a list of users, if any, that are in the specified role.
    /// </returns>
    async Task<IList<User>> IUserRoleStore<User>.GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.GetUsersInRoleAsync called for user {roleName}", normalizedRoleName);
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrWhiteSpace(normalizedRoleName))
        {
            throw new ArgumentNullException(nameof(normalizedRoleName));
        }
        using var context = contextFactory.CreateDbContext();
        var role = await context.Roles.SingleOrDefaultAsync(r => r.NormalizedName.Equals(normalizedRoleName), cancellationToken: cancellationToken);
        if (role is not null)
        {
            var query = from userrole in context.UsersRoles
                        join user in context.Users on userrole.UserId equals user.Id
                        where userrole.RoleId.Equals(role.Id)
                        select user;

            return await query.ToListAsync(cancellationToken);
        }
        return new List<User>();
    }
    #endregion

    #region IUserClaimStore
    /// <summary>
    /// Get the claims associated with the specified <paramref name="user"/> as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose claims should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a user.</returns>
    async Task<IList<Claim>> IUserClaimStore<User>.GetClaimsAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.GetClaimsAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        using var context = contextFactory.CreateDbContext();
        return await context.UserClaims.Where(uc => uc.UserId.Equals(user.Id)).Select(c => c.ToClaim()).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds the <paramref name="claims"/> given to the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to add the claim to.</param>
    /// <param name="claims">The claim to add to the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task IUserClaimStore<User>.AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.AddClaimsAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (claims == null) throw new ArgumentNullException(nameof(claims));

        using var context = contextFactory.CreateDbContext();
        foreach (var claim in claims)
        {
            UserClaim userClaim = new(user.Id, claim);
            context.UserClaims.Add(userClaim);
        }
        return Task.FromResult(context.SaveChanges());
    }

    /// <summary>
    /// Replaces the <paramref name="claim"/> on the specified <paramref name="user"/>, with the <paramref name="newClaim"/>.
    /// </summary>
    /// <param name="user">The user to replace the claim on.</param>
    /// <param name="claim">The claim replace.</param>
    /// <param name="newClaim">The new claim replacing the <paramref name="claim"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    async Task IUserClaimStore<User>.ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.ReplaceClaimAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (claim == null) throw new ArgumentNullException(nameof(claim));
        if (newClaim == null) throw new ArgumentNullException(nameof(newClaim));

        using var context = contextFactory.CreateDbContext();
        var matchedClaims = await context.UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.Value == claim.Value && uc.Type == claim.Type).ToListAsync(cancellationToken);
        foreach (var matchedClaim in matchedClaims)
        {
            matchedClaim.Value = newClaim.Value;
            matchedClaim.Type = newClaim.Type;
        }
    }

    /// <summary>
    /// Removes the <paramref name="claims"/> given from the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to remove the claims from.</param>
    /// <param name="claims">The claim to remove.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    async Task IUserClaimStore<User>.RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.RemoveClaimsAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (claims == null)
            throw new ArgumentNullException(nameof(claims));

        using var context = contextFactory.CreateDbContext();
        foreach (var claim in claims)
        {
            var matchedClaims = await context.UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.Value == claim.Value && uc.Type == claim.Type).ToListAsync(cancellationToken);
            foreach (var c in matchedClaims)
            {
                context.UserClaims.Remove(c);
            }
        }
    }

    /// <summary>
    /// Retrieves all users with the specified claim.
    /// </summary>
    /// <param name="claim">The claim whose users should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> contains a list of users, if any, that contain the specified claim.
    /// </returns>
    async Task<IList<User>> IUserClaimStore<User>.GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.GetUsersForClaimAsync called for claim {subject}", claim.Subject);
        cancellationToken.ThrowIfCancellationRequested();
        if (claim == null) throw new ArgumentNullException(nameof(claim));

        using var context = contextFactory.CreateDbContext();
        var query = from userclaims in context.UserClaims
                    join user in context.Users on userclaims.UserId equals user.Id
                    where userclaims.Value == claim.Value
                    && userclaims.Type == claim.Type
                    select user;

        return await query.ToListAsync(cancellationToken);
    }
    #endregion

    #region IQueryableUserStore
    IQueryable<User> IQueryableUserStore<User>.Users
    {
        get { return contextFactory.CreateDbContext().Set<User>(); }
    }
    #endregion

    #region IUserAuthenticationTokenStore
    /// <summary>
    /// Sets the token value for a particular user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="loginProvider">The authentication provider for the token.</param>
    /// <param name="name">The name of the token.</param>
    /// <param name="value">The value of the token.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    async Task IUserAuthenticationTokenStore<User>.SetTokenAsync(User user, string loginProvider, string name, string value, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.SetTokenAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        //var newToken = new UserToken(user.Id, loginProvider, name, value);
        //using PulseDbContext context = contextFactory.CreateDbContext();
        //context.Update<UserToken>(newToken);
        //return Task.FromResult(context.SaveChanges());

        using var context = contextFactory.CreateDbContext();
        var userToken = await context.UserTokens.FindAsync(new object[] { user.Id, loginProvider, name }, cancellationToken: cancellationToken);
        if (userToken is null)
        {
            userToken = new UserToken(user.Id, loginProvider, name, value);
            await context.UserTokens.AddAsync(userToken, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            userToken.Value = value;
            context.UserTokens.Update(userToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Removes a user token if it exists.
    /// </summary>
    /// <param name="user">The token owner.</param>
    /// <param name="loginProvider">The login provider for the token.</param>
    /// <param name="name">The name of the token.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    async Task IUserAuthenticationTokenStore<User>.RemoveTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.RemoveTokenAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        using var context = contextFactory.CreateDbContext();
        var userToken = await context.UserTokens.FindAsync(new object[] { user.Id, loginProvider, name }, cancellationToken: cancellationToken);
        if (userToken is not null)
        {
            context.UserTokens.Remove(userToken);
        }
    }

    /// <summary>
    /// Find a user token if it exists.
    /// </summary>
    /// <param name="user">The token owner.</param>
    /// <param name="loginProvider">The login provider for the token.</param>
    /// <param name="name">The name of the token.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The user token if it exists.</returns>
    async Task<string> IUserAuthenticationTokenStore<User>.GetTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.GetTokenAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (loginProvider == null)
            throw new ArgumentNullException(nameof(loginProvider));
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        using var context = contextFactory.CreateDbContext();
        var userToken = await context.UserTokens.FindAsync(new object[] { user.Id, loginProvider, name }, cancellationToken: cancellationToken);
        return userToken?.Value!;
    }
    #endregion

    #region IUserAuthenticatorKeyStore
    /// <summary>
    /// Sets the authenticator key for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose authenticator key should be set.</param>
    /// <param name="key">The authenticator key to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task IUserAuthenticatorKeyStore<User>.SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.SetAuthenticatorKeyAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.SecurityStamp = key;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Get the authenticator key for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose security stamp should be set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the security stamp for the specified <paramref name="user"/>.</returns>
    Task<string> IUserAuthenticatorKeyStore<User>.GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.GetAuthenticatorKeyAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.SecurityStamp);
    }
    #endregion

    #region IUserTwoFactorStore
    /// <summary>
    /// Sets a flag indicating whether the specified <paramref name="user"/> has two factor authentication enabled or not,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose two factor authentication enabled status should be set.</param>
    /// <param name="enabled">A flag indicating whether the specified <paramref name="user"/> has two factor authentication enabled.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task IUserTwoFactorStore<User>.SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.SetTwoFactorEnabledAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.TwoFactorEnabled = enabled;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Returns a flag indicating whether the specified <paramref name="user"/> has two factor authentication enabled or not,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose two factor authentication enabled status should be set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing a flag indicating whether the specified
    /// <paramref name="user"/> has two factor authentication enabled or not.
    /// </returns>
    Task<bool> IUserTwoFactorStore<User>.GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.GetTwoFactorEnabledAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.TwoFactorEnabled);
    }
    #endregion

    #region IUserTwoFactorRecoveryCodeStore
    private const string InternalLoginProvider = "ThePulseUserStore";
    private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
    private const string RecoveryCodeTokenName = "RecoveryCodes";

    /// <summary>
    /// Sets the token value for a particular user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="loginProvider">The authentication provider for the token.</param>
    /// <param name="name">The name of the token.</param>
    /// <param name="value">The value of the token.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    async Task SetTokenAsync(User user, string loginProvider, string name, string value, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PulseUserStore.SetTokenAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        using var context = contextFactory.CreateDbContext();
        var userToken = await context.UserTokens.FindAsync(new object[] { user.Id, loginProvider, name }, cancellationToken: cancellationToken);
        if (userToken is null)
        {
            userToken = new UserToken(user.Id, loginProvider, name, value);
            await context.UserTokens.AddAsync(userToken, cancellationToken);
        }
        else
        {
            userToken.Value = value;
        }
    }

    /// <summary>
    /// Returns how many recovery code are still valid for a user.
    /// </summary>
    /// <param name="user">The user who owns the recovery code.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The number of valid recovery codes for the user..</returns>
    async Task<int> IUserTwoFactorRecoveryCodeStore<User>.CountCodesAsync(User user, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.CountCodesAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        using var context = contextFactory.CreateDbContext();
        var userToken = await context.UserTokens.FindAsync(new object[] { user.Id, InternalLoginProvider, RecoveryCodeTokenName }, cancellationToken);
        var tokenValue = userToken?.Value;
        if (!string.IsNullOrWhiteSpace(tokenValue))
        {
            return tokenValue.Split(';').Length;
        }
        return 0;
    }

    /// <summary>
    /// Updates the recovery codes for the user while invalidating any previous recovery codes.
    /// </summary>
    /// <param name="user">The user to store new recovery codes for.</param>
    /// <param name="recoveryCodes">The new recovery codes for the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The new recovery codes for the user.</returns>
    Task IUserTwoFactorRecoveryCodeStore<User>.ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.ReplaceCodesAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var mergedCodes = string.Join(";", recoveryCodes);
        return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
    }

    /// <summary>
    /// Returns whether a recovery code is valid for a user. Note: recovery codes are only valid
    /// once, and will be invalid after use.
    /// </summary>
    /// <param name="user">The user who owns the recovery code.</param>
    /// <param name="code">The recovery code to use.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>True if the recovery code was found for the user.</returns>
    async Task<bool> IUserTwoFactorRecoveryCodeStore<User>.RedeemCodeAsync(User user, string code, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserStore.CountCodesAsync called for user {userName}", user.UserName);
        cancellationToken.ThrowIfCancellationRequested();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (code is null)
        {
            throw new ArgumentNullException(nameof(code));
        }
        using var context = contextFactory.CreateDbContext();
        var userToken = await context.UserTokens.FindAsync(new object[] { user.Id, InternalLoginProvider, RecoveryCodeTokenName }, cancellationToken);

        var splitCodes = userToken?.Value.Split(';');
        if (splitCodes?.Length > 0 && splitCodes.Contains(code))
        {
            var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
            var mergedCodes = string.Join(";", updatedCodes);
            await SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
            return true;
        }
        return false;
    }
    #endregion


    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposedValue = true;
        }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~UserStore2()
    // {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        // TODO: uncomment the following line if the finalizer is overridden above.
        // GC.SuppressFinalize(this);
    }


    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        // TODO: uncomment the following line if the finalizer is overridden above.
        // GC.SuppressFinalize(this);
    }

    #endregion
}
