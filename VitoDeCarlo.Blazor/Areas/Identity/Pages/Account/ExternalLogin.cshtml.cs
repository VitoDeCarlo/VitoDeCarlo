using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VitoDeCarlo.Models.Identity;
using System;

namespace VitoDeCarlo.Blazor.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ExternalLoginModel : PageModel
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ExternalLoginModel> _logger;

    public ExternalLoginModel(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        ILogger<ExternalLoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = null!;

    public string LoginProvider { get; set; } = null!;

    public string ProviderDisplayName { get; set; } = null!;

    public string ReturnUrl { get; set; } = null!;

    [TempData]
    public string ErrorMessage { get; set; } = null!;

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }

    public IActionResult OnGetAsync()
    {
        return RedirectToPage("./Login");
    }

    public IActionResult OnPost(string provider, string returnUrl)
    {
        // Request a redirect to the external login provider.
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl, string remoteError)
    {
        returnUrl ??= Url.Content("~/");
        if (remoteError != null)
        {
            ErrorMessage = $"Error from external provider: {remoteError}";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Error loading external login information.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (result.Succeeded)
        {
            _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal?.Identity?.Name, info.LoginProvider);
            return LocalRedirect(returnUrl);
        }
        if (result.IsLockedOut)
        {
            return RedirectToPage("./Lockout");
        }
        else
        {
            // If the user does not have an account, then ask the user to create an account.
            ReturnUrl = returnUrl;
            LoginProvider = info.LoginProvider;
            ProviderDisplayName = info.ProviderDisplayName;
            var p = info.Principal;
            if (p.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                var user = new User
                {
                    Email = p.FindFirstValue(ClaimTypes.Email),
                    EmailConfirmed = true
                };

                if (p.HasClaim(c => c.Type == ClaimTypes.GivenName))
                {
                    user.GivenName = p.FindFirstValue(ClaimTypes.GivenName);
                }
                if (p.HasClaim(c => c.Type == ClaimTypes.Surname))
                {
                    user.FamilyName = p.FindFirstValue(ClaimTypes.Surname);
                }

                // Username
                if (!string.IsNullOrWhiteSpace(user.GivenName) && !string.IsNullOrWhiteSpace(user.FamilyName))
                {
                    Random random = new();
                    int num = random.Next(1000);
                    string username = user.GivenName + user.FamilyName + num.ToString();
                    user.UserName = username;
                }
                else
                {
                    Random random = new();
                    int num = random.Next(1000);
                    string username = string.Concat(user.Email.AsSpan(0, user.Email.IndexOf('@')), num.ToString());
                    user.UserName = username;
                }

                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    createResult = await _userManager.AddLoginAsync(user, info);
                    if (createResult.Succeeded)
                    {
                        // Copy over the claims
                        await _userManager.AddClaimsAsync(user, info.Principal.Claims);

                        // Include the access token in the properties (new)
                        var authProperties = new AuthenticationProperties();
                        authProperties.StoreTokens(info.AuthenticationTokens);
                        authProperties.IsPersistent = true;

                        await _signInManager.SignInAsync(user, authProperties);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return Page();
        }
    }

    /// <summary>
    /// If the User is new and needs to register (by adding an email), this post is called after the user submits email.
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl)
    {
        returnUrl ??= Url.Content("~/");
        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Error loading external login information during confirmation.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        if (ModelState.IsValid)
        {
            var user = new User { UserName = Input.Email, Email = Input.Email };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    // Copy over the claims
                    await _userManager.AddClaimsAsync(user, info.Principal.Claims);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                    return LocalRedirect(returnUrl);
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        LoginProvider = info.LoginProvider;
        ProviderDisplayName = info.ProviderDisplayName;
        ReturnUrl = returnUrl;
        return Page();
    }
}
