namespace VitoDeCarlo.Models.Geography;

public class Region : BaseGeographyEntity
{
    public string CountryId { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;
}
