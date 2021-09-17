namespace VitoDeCarlo.Models.Identity;

public class UserLogin
{
    public string LoginProvider { get; set; } = null!;

    public string ProviderDisplayName { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public long UserId { get; set; }

    public DateTime LoginTime { get; set; }
}
