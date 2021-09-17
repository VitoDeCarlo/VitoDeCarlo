using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VitoDeCarlo.Data.Repositories.Base;
using VitoDeCarlo.Models.Geography;

namespace VitoDeCarlo.Data.Repositories;

public class CountryRepo : BaseGeographyRepo<Country>, ICountryRepo
{
    public CountryRepo(IDbContextFactory<VitoDbContext> contextFactory) : base(contextFactory) { }

    public override Country? Find(string id)
    {
        using var context = ContextFactory.CreateDbContext();
        return context.Countries
            .IgnoreQueryFilters()
            .Where(x => x.Id == id)
            .Include(c => c.Regions)
            .FirstOrDefault();
    }

    public override async Task<Country?> FindAsync(string id)
    {
        using var context = ContextFactory.CreateDbContext();
        return await context.Countries
            .IgnoreQueryFilters()
            .Include(c => c.Regions)
            .SingleAsync(x => x.Id == id);
    }
}
