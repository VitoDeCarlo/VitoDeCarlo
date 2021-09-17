namespace VitoDeCarlo.Models.Geography;

public abstract class BaseGeographyEntity
{
    public string Id { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
