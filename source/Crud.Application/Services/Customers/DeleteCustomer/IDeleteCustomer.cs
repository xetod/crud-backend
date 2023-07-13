using Crud.Application.Core.Result;
using Crud.Domain.Entities;

namespace Crud.Application.Services.Customers.DeleteCustomer;

public interface IDeleteCustomer
{
    Task<Result> ExecuteAsync(int customerId);
}