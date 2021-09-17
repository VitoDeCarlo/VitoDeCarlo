using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VitoDeCarlo.Core.Services;
using VitoDeCarlo.Models.Identity;

namespace VitoDeCarlo.Blazor.Areas.Identity.Pages.Account.Manage;

public partial class IndexModel : PageModel
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly IUserService _userService;
    private readonly TwilioVerifyService _twilioService;

    public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, IUserService userService, TwilioVerifyService twilioService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _userService = userService;
        _twilioService = twilioService;
    }

    public bool IsEmailConfirmed { get; set; }

    public bool IsPhoneSaved { get; set; }

    public bool IsPhoneConfirmed { get; set; }

    [TempData]
    public string StatusMessage { get; set; } = null!;

    [BindProperty]
    public InputModel Input { get; set; } = null!;

    public class InputModel
    {
        [Required]
        [Display(Name = "Username")]
        [MaxLength(25, ErrorMessage = "{0} must not be more than {1} characters.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [MaxLength(100, ErrorMessage = "{0} must not be more than {1} characters.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First/Given/Forename")]
        [StringLength(50, ErrorMessage = "The {0} must be no longer than {1} characters long.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last/Family/Surname")]
        [StringLength(50, ErrorMessage = "The {0} must be no longer than {1} characters long.")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Dialing Code")]
        [MaxLength(3, ErrorMessage = "{0} must not be more than {1} numbers.")]
        [RegularExpression(@"([1-9][0-9]?[0-9]?)", ErrorMessage = "{0} must contain numbers only and not start with 0")]
        public string? DialingCode { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        [MaxLength(15, ErrorMessage = "{0} must not be more than {1} characters.")]
        [RegularExpression(@"[0-9- ]{1,15}", ErrorMessage = "Include your Country Dialing Code")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string CountryCode { get; set; } = string.Empty;

        [Display(Name = "State/Province/Region")]
        public string RegionCode { get; set; } = string.Empty;

        [Display(Name = "Zip/Postal Code")]
        public string PostalCode { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        Input = new InputModel
        {
            Username = user.UserName,
            FirstName = user.GivenName ?? string.Empty,
            LastName = user.FamilyName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            DialingCode = user.DialingCode ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            CountryCode = user.CountryCode ?? string.Empty,
            RegionCode = user.RegionCode ?? string.Empty,
            PostalCode = user.PostalCode ?? string.Empty
        };

        IsEmailConfirmed = user.EmailConfirmed;
        IsPhoneSaved = !string.IsNullOrWhiteSpace(user.PhoneNumber);
        IsPhoneConfirmed = user.PhoneNumberConfirmed;

        if (!string.IsNullOrWhiteSpace(user.CountryCode) && string.IsNullOrWhiteSpace(user.DialingCode))
        {
            Input.DialingCode = _userService.GetDialingCode(user.CountryCode);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        user.UserName = Input.Username;
        user.NormalizedUserName = _userManager.NormalizeName(Input.Username);
        user.GivenName = Input.FirstName;
        user.FamilyName = Input.LastName;
        user.CountryCode = Input.CountryCode;
        user.RegionCode = Input.RegionCode;
        user.PostalCode = Input.PostalCode;
        user.DialingCode = Input.DialingCode;

        if (Input.Email != user.Email)
        {
            var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
            if (!setEmailResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
            }
        }

        if (Input.PhoneNumber != user.PhoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }
        }

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            StatusMessage = "Your profile has been updated";
        }
        else
        {
            StatusMessage = "There was an error while updating your profile.";
        }
        return RedirectToPage();
    }


    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code, Request.Scheme);
        await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);

        StatusMessage = "Verification email sent. Please check your email.";
        return RedirectToPage();
    }


    public async Task<IActionResult> OnPostVerifyPhoneAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        try
        {
            int.TryParse(Input.DialingCode, out int dialingCode);
            var result = await _twilioService.StartVerification(dialingCode, Input.PhoneNumber);
            if (result.Success)
            {
                return RedirectToPage("ConfirmPhone", new { Input.DialingCode, Input.PhoneNumber });
            }

            ModelState.AddModelError("", $"There was an error sending the verification code: {result.Message}");
        }
        catch (Exception)
        {
            ModelState.AddModelError("",
                "There was an error sending the verification code, please check the phone number is correct and try again");
        }

        return Page();
    }

    public async Task<IEnumerable<SelectListItem>> GetCountries()
    {
        var countryList = await _userService.GetCountriesAsync();
        var selectList = countryList.Prepend(new KeyValuePair<string, string>("--- Select Country ---", string.Empty));
        return selectList.Select(c => new SelectListItem { Text = c.Key, Value = c.Value });
    }

    public async Task<IEnumerable<SelectListItem>> GetRegions()
    {
        var regionList = await _userService.GetRegionsAsync(Input.CountryCode);
        var selectList = regionList.Prepend(new KeyValuePair<string, string>("--- Select Region ---", string.Empty));
        return selectList.Select(r => new SelectListItem { Text = r.Key, Value = r.Value });
    }
}
