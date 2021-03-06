using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VitoDeCarlo.Models.Identity;

namespace VitoDeCarlo.Blazor.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LogOutModel : PageModel
{
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<LogOutModel> _logger;

    public LogOutModel(SignInManager<User> signInManager, ILogger<LogOutModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            return RedirectToPage();
        }
    }
}
