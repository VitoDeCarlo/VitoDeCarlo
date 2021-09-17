using Microsoft.EntityFrameworkCore;
using VitoDeCarlo.Data.Exceptions;
using VitoDeCarlo.Models.Geography;

namespace VitoDeCarlo.Data.Repositories.Base;

public abstract class BaseGeographyRepo<T> : IGeographyRepo<T> where T : BaseGeographyEntity, new()
{
    public IDbContextFactory<VitoDbContext> ContextFactory { get; }

    protected BaseGeographyRepo(IDbContextFactory<VitoDbContext> contextFactory)
    {
        ContextFactory = contextFactory;
    }

    public virtual int Add(T entity, bool persist = true)
    {
        using var context = ContextFactory.CreateDbContext();
        context.Set<T>().Add(entity);
        return persist ? SaveChanges() : 0;
    }

    public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
    {
        using var context = ContextFactory.CreateDbContext();
        context.Set<T>().AddRange(entities);
        return persist ? SaveChanges() : 0;
    }

    public virtual T? Find(string id)
    {
        using var context = ContextFactory.CreateDbContext();
        return context.Set<T>().Find(id);
    }

    public virtual async Task<T?> FindAsync(string id)
    {
        using var context = ContextFactory.CreateDbContext();
        return await context.Set<T>().FindAsync(id);
    }

    public virtual T? FindAsNoTracking(string id)
    {
        using var context = ContextFactory.CreateDbContext();
        return context.Set<T>()
            .AsNoTrackingWithIdentityResolution()
            .Single(x => x.Id == id);
    }

    public virtual T? FindIgnoreQueryFilters(string id)
    {
        using var context = ContextFactory.CreateDbContext();
        return context.Set<T>()
            .IgnoreQueryFilters()
            .Single(x => x.Id == id);
    }

    public virtual IEnumerable<T> GetAll()
    {
        using var context = ContextFactory.CreateDbContext();
        return context.Set<T>();
    }

    public virtual IEnumerable<T> GetAllIgnoreQueryFilters()
    {
        using var context = ContextFactory.CreateDbContext();
        return context.Set<T>().IgnoreQueryFilters();
    }

    public virtual int Update(T entity, bool persist = true)
    {
        using var context = ContextFactory.CreateDbContext();
        context.Set<T>().Update(entity);
        return persist ? SaveChanges() : 0;
    }

    public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
    {
        using var context = ContextFactory.CreateDbContext();
        context.Set<T>().UpdateRange(entities);
        return persist ? SaveChanges() : 0;
    }

    public virtual int Delete(T entity, bool persist = true)
    {
        using var context = ContextFactory.CreateDbContext();
        context.Set<T>().Remove(entity);
        return persist ? SaveChanges() : 0;
    }

    public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
    {
        using var context = ContextFactory.CreateDbContext();
        context.Set<T>().RemoveRange(entities);
        return persist ? SaveChanges() : 0;
    }

    public int SaveChanges()
    {
        using var context = ContextFactory.CreateDbContext();
        try
        {
            return context.SaveChanges();
        }
        catch (CustomException)
        {
            // ToDo: handle exception (already logged)
            throw;
        }
        catch (Exception ex)
        {
            // ToDo: Should log and handle
            throw new CustomException("An error occurred updating the database.", ex);
        }
    }
}
