using Microsoft.EntityFrameworkCore;

namespace Crud.Data.Repositories.Core.Repositories;

public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
{
    protected readonly DbContext Context;

    public Repository(DbContext context)
    {
        Context = context;
    }

    public void Delete(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}