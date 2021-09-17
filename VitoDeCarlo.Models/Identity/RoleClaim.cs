using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace VitoDeCarlo.Models.Identity;

public class RoleClaim
{
    [Key]
    public int Id { get; set; }

    public long RoleId { get; set; }

    public string Type { get; set; }

    public string Value { get; set; }

    public string ValueType { get; set; }

    public string Issuer { get; set; }

    public string OriginalIssuer { get; set; }


    public RoleClaim(long roleId, Claim claim)
    {
        RoleId = roleId;
        Type = claim.Type;
        Value = claim.Value;
        ValueType = claim.ValueType;
        Issuer = claim.Issuer;
        OriginalIssuer = claim.OriginalIssuer;
    }

    public RoleClaim(long roleId, string type, string value)
    {
        RoleId = roleId;
        Type = type;
        Value = value;
        ValueType = ClaimValueTypes.String;
        Issuer = ClaimsIdentity.DefaultIssuer;
        OriginalIssuer = ClaimsIdentity.DefaultIssuer;
    }

    public RoleClaim(long roleId, string type, string value, string valueType)
    {
        RoleId = roleId;
        Type = type;
        Value = value;
        ValueType = valueType;
        Issuer = ClaimsIdentity.DefaultIssuer;
        OriginalIssuer = ClaimsIdentity.DefaultIssuer;
    }

    public RoleClaim(long roleId, string type, string value, string valueType, string issuer)
    {
        RoleId = roleId;
        Type = type;
        Value = value;
        ValueType = valueType;
        Issuer = issuer;
        OriginalIssuer = issuer;
    }

    public RoleClaim(long roleId, string type, string value, string valueType, string issuer, string originalIssuer)
    {
        RoleId = roleId;
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
