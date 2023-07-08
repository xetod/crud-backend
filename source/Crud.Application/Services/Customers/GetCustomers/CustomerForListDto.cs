namespace Crud.Application.Services.Customers.GetCustomers;

using System.Collections.Generic;

/// <summary>
/// Data transfer object (DTO) for representing a customer in a list.
/// </summary>
public class CustomerForListDto
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the first name of the customer.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the customer.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the email address of the customer.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the address of the customer.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the list of sales associated with the customer.
    /// </summary>
    public ICollection<SalesForListDto> Sales { get; set; }
}
