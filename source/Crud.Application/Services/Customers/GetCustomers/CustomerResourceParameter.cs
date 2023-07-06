using Crud.Application.Core.ResourceParameters;

namespace Crud.Application.Services.Customers.GetCustomers;

public class CustomerResourceParameter : ResourceParameter
{
    public string SortBy { get; set; } = "Name";

    public string CustomerName { get; set; }
}