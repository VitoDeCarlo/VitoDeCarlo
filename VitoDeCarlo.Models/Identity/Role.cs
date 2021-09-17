namespace VitoDeCarlo.Models.Identity;

public class Role
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public string ConcurrencyStamp { get; set; } = null!;

    public Role() { }

    public Role(string roleName)
    {
        Name = roleName;
    }
}
