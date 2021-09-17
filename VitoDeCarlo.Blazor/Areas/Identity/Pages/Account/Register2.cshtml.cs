using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VitoDeCarlo.Core.Services;
using VitoDeCarlo.Models.Identity;
using VitoDeCarlo.Data;

namespace VitoDeCarlo.Blazor.Areas.Identity.Pages.Account;

public class Register2Model : PageModel
{
    private readonly IDbContextFactory<VitoDbContext> contextFactory;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<LoginModel> _logger;
    private readonly IEmailSender _emailSender;

    public Register2Model(
        IDbContextFactory<VitoDbContext> contextFactory, 
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<LoginModel> logger,
        IEmailSender emailSender)
    {
        this.contextFactory = contextFactory;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _emailSender = emailSender;
    }

    [BindProperty]
    public InputModel Input { get; set; } = null!;

    [BindProperty]
    public string ReturnUrl { get; set; } = string.Empty;

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
        [Display(Name = "Password")]
        [MinLength(8, ErrorMessage = "{0} must be at least {1} characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First/Given/Forename")]
        [StringLength(50, ErrorMessage = "The {0} must be no longer than {1} characters long.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last/Family/Surname")]
        [StringLength(50, ErrorMessage = "The {0} must be no longer than {1} characters long.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Country")]
        public string CountryCode { get; set; } = string.Empty;

        [Display(Name = "State/Province/Region")]
        public string RegionCode { get; set; } = string.Empty;

        [Display(Name = "Zip/Postal Code")]
        public string PostalCode { get; set; } = string.Empty;
    }

    public void OnGet(string returnUrl)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl)
    {
        ReturnUrl = returnUrl;
        if (ModelState.IsValid)
        {
            var user = new User
            {
                UserName = Input.Username,
                Email = Input.Email,
                GivenName = Input.FirstName,
                FamilyName = Input.LastName,
                CountryCode = Input.CountryCode,
                RegionCode = Input.RegionCode,
                PostalCode = Input.PostalCode
            };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code, Request.Scheme);
                await _emailSender.SendEmailConfirmationAsync(Input.Email, callbackUrl);

                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }


    public IList<SelectListItem> GetCountries()
    {
        using var context = contextFactory.CreateDbContext();
        List<SelectListItem> countries = context.Countries.AsQueryable().OrderBy(c => c.Name).Select(c => new SelectListItem { Text = c.Name, Value = c.Code }).ToList();
        var countryTip = new SelectListItem() { Text = "--- Select Country ---", Value = null };
        countries.Insert(0, countryTip);
        return countries;
    }

    public JsonResult OnGetRegions(string countryCode)
    {
        using var context = contextFactory.CreateDbContext();
        var regionList = new SelectList(context.Regions.Where(r => r.Code.Equals(countryCode)), "RegionCode", "Name");
        return new JsonResult(regionList);
    }
}
