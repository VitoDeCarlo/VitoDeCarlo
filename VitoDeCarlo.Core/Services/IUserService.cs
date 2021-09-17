using Microsoft.AspNetCore.Mvc.Rendering;

namespace VitoDeCarlo.Core.Services;

public interface IUserService
{
    IList<SelectListItem> GetCountries();

    Task<IDictionary<string, string>> GetCountriesAsync();

    Task<IDictionary<string, string>> GetRegionsAsync(string countryCode);

    string GetDialingCode(string countryCode);
}
