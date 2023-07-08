namespace Crud.Data.Core.Specifications;

/// <summary>
/// This class can be used to configure the sorting behavior for a collection of entities based on specific criteria and sort order.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public class Sort<T>
{
    /// <summary>
    /// Gets or sets the specification used for sorting.
    /// </summary>
    public Specification<T> Specification { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the sorting is in ascending order.
    /// </summary>
    public bool Ascending { get; set; }
}