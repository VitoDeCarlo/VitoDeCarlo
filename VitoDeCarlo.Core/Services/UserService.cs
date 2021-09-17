using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VitoDeCarlo.Data;

namespace VitoDeCarlo.Core.Services;

public class UserService : IUserService
{
    private readonly IDbContextFactory<VitoDbContext> contextFactory;

    public UserService(IDbContextFactory<VitoDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public IList<SelectListItem> GetCountries()
    {
        using (var context = contextFactory.CreateDbContext())
        {
            List<SelectListItem> countries = context.Countries.OrderBy(c => c.Name).Select(c => new SelectListItem { Text = c.Name, Value = c.Code }).ToList();
            //List<SelectListItem> countries = context.Countries.AsQueryable().OrderBy(c => c.Name).Select(c => new SelectListItem { Text = c.Name, Value = c.ID }).ToList();
            var countryTip = new SelectListItem() { Text = "--- Select Country ---", Value = null };
            countries.Insert(0, countryTip);
            return countries;
        }
    }

    public async Task<IDictionary<string, string>> GetCountriesAsync()
    {
        using (var context = contextFactory.CreateDbContext())
        {
            var countryList = await context.Countries.OrderBy(c => c.Name).ToDictionaryAsync(c => c.Name, c => c.Code);
            countryList.Add("--- Select Country ---", null);
            return countryList;
        }
    }

    public async Task<IDictionary<string, string>> GetRegionsAsync(string countryCode)
    {
        using (var context = contextFactory.CreateDbContext())
        {
            //return await context.Regions.Where(r => r.CountryID == countryCode.ToLower()).ToDictionaryAsync(r => r.Name, r => r.ID);
            //var something = context.Regions.Where(r => r.CountryID == countryCode.ToLower()).ToDictionaryAsync(r => r.Name, r => r.ID);
            return await context.Regions.Where(r => r.Country.Code == countryCode).ToDictionaryAsync(r => r.Name, r => r.Code);
        }
    }

    public string GetDialingCode(string countryCode)
    {
        using (var context = contextFactory.CreateDbContext())
        {
            return context.Countries.Where(c => c.Code == countryCode).Select(c => c.DialingCode).FirstOrDefault().ToString();
        }
    }
}
