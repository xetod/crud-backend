namespace Crud.Domain.Entities;

/// <summary>
/// Represents a customer entity.
/// </summary>
public class Customer
{
    public Customer()
    {
        Sales = new List<Sale>();
    }

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
    /// Gets or sets the phone number of the customer.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the sales associated with the customer.
    /// </summary>
    public ICollection<Sale> Sales { get; set; }

    /// <summary>
    /// Gets the full name of the customer by combining the first name and last name.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    public bool IsEmpty() => CustomerId == -1;
}