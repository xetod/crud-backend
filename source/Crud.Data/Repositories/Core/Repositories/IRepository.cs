namespace Crud.Data.Repositories.Core.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    void Delete(TEntity entity);

    Task AddAsync(TEntity entity);
}