namespace Crud.Application.Core.ResourceParameters;

/// <summary>
/// Represents the pagination metadata for a collection of resources.
/// </summary>
public class PaginationMetadata
{
    /// <summary>
    /// Gets or sets the total count of resources in the collection.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the size of each page in the collection.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages in the collection.
    /// </summary>
    public int TotalPages { get; set; }
}
