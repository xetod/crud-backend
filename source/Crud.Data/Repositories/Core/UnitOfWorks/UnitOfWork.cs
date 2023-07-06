using Crud.Data.DbContexts;
using Crud.Data.Repositories.Customers;

namespace Crud.Data.Repositories.Core.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly CrudDbContext _crudDbContext;

    public UnitOfWork(CrudDbContext crudDbContext)
    {
        _crudDbContext = crudDbContext;

        Customer = new CustomerRepository(_crudDbContext);
    }

    public ICustomerRepository Customer { get; }


    public async Task<int> CompleteAsync()
    {
            return await _crudDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _crudDbContext.Dispose();
    }
}