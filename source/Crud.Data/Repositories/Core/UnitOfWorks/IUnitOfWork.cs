using Crud.Data.Repositories.Customers;

namespace Crud.Data.Repositories.Core.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customer { get; }

    Task<int> CompleteAsync();
}