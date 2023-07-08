namespace Crud.Application.Core.ResourceParameters;

/// <summary>
/// Represents the parameters for querying a resource collection.
/// </summary>
public class ResourceParameter
{
    /// <summary>
    /// Gets or sets the current page number.
    /// Default value is 1.
    /// </summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size, i.e., the number of items per page.
    /// Default value is 10.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets a value indicating whether the sorting order is ascending or descending.
    /// Default value is true, indicating ascending order.
    /// </summary>
    public bool IsAscending { get; set; } = true;
}
