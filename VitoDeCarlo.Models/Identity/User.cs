using System.ComponentModel.DataAnnotations;

namespace VitoDeCarlo.Models.Identity;

public class User
{
    public long Id { get; set; }

    public string UserName { get; set; } = null!;

    public string NormalizedUserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string NormalizedEmail { get; set; } = null!;

    public string? GivenName { get; set; }

    public string? FamilyName { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? RegionCode { get; set; }

    public string? CountryCode { get; set; }

    [DataType(DataType.PostalCode)]
    public string? PostalCode { get; set; }

    public string? DialingCode { get; set; }

    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    public string? Gender { get; set; }

    [DataType(DataType.Date), Display(Name = "Birth Date")]
    [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? BirthDate { get; set; }

    public bool Newsletter { get; set; }

    public DateTime RegisterDate { get; set; }




    public bool IsAuthenticated { get; set; }

    public string AuthenticationType { get; set; } = null!;

    public bool EmailConfirmed { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public bool LockoutEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

    public string SecurityStamp { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}
