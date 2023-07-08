using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.GetCustomer.Models;

namespace Crud.Application.Services.Customers.GetCustomer;

public interface IGetCustomer
{
    Task<Result<CustomerForDetailDto>> ExecuteAsync(int customerId);
}