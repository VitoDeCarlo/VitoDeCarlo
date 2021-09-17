using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace VitoDeCarlo.Models.Identity;

public class UserClaim
{
    [Key]
    public int Id { get; set; }

    public long UserId { get; set; }

    public string Type { get; set; }

    public string Value { get; set; }

    public string ValueType { get; set; }

    public string Issuer { get; set; }

    public string OriginalIssuer { get; set; }


    public UserClaim(long userId, Claim claim)
    {
        UserId = userId;
        Type = claim.Type;
        Value = claim.Value;
        ValueType = claim.ValueType;
        Issuer = claim.Issuer;
        OriginalIssuer = claim.OriginalIssuer;
    }

    public UserClaim(long userId, string type, string value)
    {
        UserId = userId;
        Type = type;
        Value = value;
        ValueType = ClaimValueTypes.String;
        Issuer = ClaimsIdentity.DefaultIssuer;
        OriginalIssuer = ClaimsIdentity.DefaultIssuer;
    }

    public UserClaim(long userId, string type, string value, string valueType)
    {
        UserId = userId;
        Type = type;
        Value = value;
        ValueType = valueType;
        Issuer = ClaimsIdentity.DefaultIssuer;
        OriginalIssuer = ClaimsIdentity.DefaultIssuer;
    }

    public UserClaim(long userId, string type, string value, string valueType, string issuer)
    {
        UserId = userId;
        Type = type;
        Value = value;
        ValueType = valueType;
        Issuer = issuer;
        OriginalIssuer = issuer;
    }

    public UserClaim(long userId, string type, string value, string valueType, string issuer, string originalIssuer)
    {
        UserId = userId;
        Type = type;
        Value = value;
        ValueType = valueType;
        Issuer = issuer;
        OriginalIssuer = originalIssuer;
    }

    public void InitializeFromClaim(Claim claim)
    {
        Type = claim.Type;
        Value = claim.Value;
        ValueType = claim.ValueType;
        Issuer = claim.Issuer;
        OriginalIssuer = claim.OriginalIssuer;
    }

    public Claim ToClaim()
    {
        var claim = new Claim(Type, Value, ValueType, Issuer, OriginalIssuer);
        return claim;
    }
}
