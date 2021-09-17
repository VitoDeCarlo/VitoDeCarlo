namespace VitoDeCarlo.Models.Identity;

public class UserToken
{
    public UserToken(long userId, string loginProvider, string name, string value)
    {
        UserId = userId;
        LoginProvider = loginProvider;
        Name = name;
        Value = value;
    }

    public long UserId { get; set; }

    public string LoginProvider { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }
}
