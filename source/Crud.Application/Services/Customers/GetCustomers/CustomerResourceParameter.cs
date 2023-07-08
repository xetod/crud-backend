using Crud.Application.Core.ResourceParameters;

namespace Crud.Application.Services.Customers.GetCustomers;

/// <summary>
/// Represents the parameters for querying a collection of customers.
/// Inherits from the base <see cref="ResourceParameter"/> class.
/// </summary>
public class CustomerResourceParameter : ResourceParameter
{
    /// <summary>
    /// Gets or sets the property to sort the customers by.
    /// Default value is "Name".
    /// </summary>
    public string SortBy { get; set; } = "Name";

    /// <summary>
    /// Gets or sets the search text to filter customers by.
    /// </summary>
    public string SearchText { get; set; }
}
