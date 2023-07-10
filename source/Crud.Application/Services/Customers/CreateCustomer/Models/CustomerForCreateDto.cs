namespace Crud.Application.Services.Customers.CreateCustomer.Models;

/// <summary>
/// Data transfer object (DTO) for representing a customer's creation.
/// </summary>
public class CustomerForCreateDto
{
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
    /// Gets or sets the phone number of the customer.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the sales associated with the customer.
    /// </summary>
    public List<SaleForCreateDto> Sales { get; set; }
}