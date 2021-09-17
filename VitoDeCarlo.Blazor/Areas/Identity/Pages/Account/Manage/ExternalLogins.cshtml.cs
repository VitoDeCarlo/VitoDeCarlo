using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VitoDeCarlo.Models.Identity;

namespace VitoDeCarlo.Blazor.Areas.Identity.Pages.Account.Manage;

public class ExternalLoginsModel : PageModel
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public ExternalLoginsModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IList<UserLoginInfo> CurrentLogins { get; set; } = null!;

    public IList<AuthenticationScheme> OtherLogins { get; set; } = null!;

    public bool ShowRemoveButton { get; set; }

    [TempData]
    public string StatusMessage { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID 'user.Id'.");
        }

        //var cancellationToken = new CancellationToken();
        //CurrentLogins = await userStore.GetLoginsAsync(user, cancellationToken);
        CurrentLogins = await _userManager.GetLoginsAsync(user);
        OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
            .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
            .ToList();
        ShowRemoveButton = user.PasswordHash != null || CurrentLogins.Count > 1;
        return Page();
    }

    public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID 'user.Id'.");
        }

        var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
        if (!result.Succeeded)
        {
            StatusMessage = "The external login was not removed.";
            return RedirectToPage();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "The external login was removed.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
    {
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // Request a redirect to the external login provider to link a login for the current user
        var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID 'user.Id'.");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
        if (info is null)
        {
            throw new InvalidOperationException($"Unexpected error occurred loading external login info for user with ID {user.Id}");
        }

        var result = await _userManager.AddLoginAsync(user, info);
        if (!result.Succeeded)
        {
            StatusMessage = "The external login was not added. External logins can only be associated with one account.";
            return RedirectToPage();
        }


        // ********** NEW PROCESS **********
        var p = info.Principal;
        if (p.HasClaim(c => c.Type.Equals(ClaimTypes.Email)))
        {
            if (p.HasClaim(c => c.Type.Equals(ClaimTypes.GivenName)))
            {
                user.GivenName = p.FindFirstValue(ClaimTypes.GivenName);
            }
            if (p.HasClaim(c => c.Type.Equals(ClaimTypes.Surname)))
            {
                user.FamilyName = p.FindFirstValue(ClaimTypes.Surname);
            }

            // Copy over the claims
            await _userManager.AddClaimsAsync(user, p.Claims);
        }


        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        StatusMessage = "The external login was added.";
        return RedirectToPage();
    }
}
