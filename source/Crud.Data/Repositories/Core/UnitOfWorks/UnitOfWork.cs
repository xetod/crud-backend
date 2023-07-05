using Crud.Data.DbContexts;

namespace Crud.Data.Repositories.Core.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly CrudDbContext _crudDbContext;

    public UnitOfWork(CrudDbContext crudDbContext)
    {
        _crudDbContext = crudDbContext;
    }


    public async Task<int> CompleteAsync()
    {
            return await _crudDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _crudDbContext.Dispose();
    }
}