using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VitoDeCarlo.Models.Identity;
using VitoDeCarlo.Core.Services;

namespace VitoDeCarlo.Blazor.Areas.Identity.Pages.Account.Manage;

[Authorize]
public class ConfirmPhoneModel : PageModel
{
    private readonly TwilioVerifyService _client;
    private readonly UserManager<User> _userManager;

    public ConfirmPhoneModel(TwilioVerifyService client, UserManager<User> userManager)
    {
        _client = client;
        _userManager = userManager;
    }

    [BindProperty(SupportsGet = true)]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required]
        [Display(Name = "Country dialing code")]
        public int DialingCode { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Code")]
        public string VerificationCode { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await _client.CheckVerificationCode(Input.DialingCode, Input.PhoneNumber, Input.VerificationCode);
            if (result.Success)
            {
                var identityUser = await _userManager.GetUserAsync(User);
                identityUser.PhoneNumberConfirmed = true;
                var updateResult = await _userManager.UpdateAsync(identityUser);

                if (updateResult.Succeeded)
                {
                    return RedirectToPage("ConfirmPhoneSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "There was an error confirming the verification code, please try again");
                }
            }
            else
            {
                ModelState.AddModelError("", $"There was an error confirming the verification code: {result.Message}");
            }
        }
        catch (Exception)
        {
            ModelState.AddModelError("",
                "There was an error confirming the code, please check the verification code is correct and try again");
        }

        return Page();
    }
}
