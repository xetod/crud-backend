using Crud.Domain.Entities.Sales;

namespace Crud.Domain.Entities.Customers;

/// <summary>
/// Represents a null customer entity.
/// </summary>
public class NullCustomer : Customer
{
    public NullCustomer()
    {
        // Set default values or behaviors for the properties and methods
        CustomerId = -1;
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        PhoneNumber = string.Empty;
        Sales = new List<Sale>();
    }
}