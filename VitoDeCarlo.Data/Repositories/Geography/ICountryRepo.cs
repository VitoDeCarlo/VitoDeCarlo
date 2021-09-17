using VitoDeCarlo.Data.Repositories.Base;
using VitoDeCarlo.Models.Geography;

namespace VitoDeCarlo.Data.Repositories;

public interface ICountryRepo : IGeographyRepo<Country>
{
}
