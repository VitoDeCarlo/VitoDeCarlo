namespace VitoDeCarlo.Models.Geography;

public class Country : BaseGeographyEntity
{
    public string Code3 { get; set; } = null!;

    public string Numeric {  get; set; } = null!;

    public string OfficialName { get; set; } = null!;

    public string DialingCode { get; set; } = null!;

    public virtual ICollection<Region> Regions { get; set; } = null!;
}
