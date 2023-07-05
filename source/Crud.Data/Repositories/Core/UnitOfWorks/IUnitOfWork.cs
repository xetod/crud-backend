namespace Crud.Data.Repositories.Core.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    Task<int> CompleteAsync();
}