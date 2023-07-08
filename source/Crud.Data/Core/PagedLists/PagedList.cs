using Microsoft.EntityFrameworkCore;

namespace Crud.Data.Core.PagedLists;

/// <summary>
/// Represents a paged list of items.
/// </summary>
/// <typeparam name="T">The type of items in the paged list.</typeparam>
public class PagedList<T> : List<T>
{
    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int CurrentPage { get; private set; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; private set; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Gets the total number of items in the paged list.
    /// </summary>
    public int TotalCount { get; private set; }

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    public bool HasPrevious => (CurrentPage > 1);

    /// <summary>
    /// Gets a value indicating whether there is a next page.
    /// </summary>
    public bool HasNext => (CurrentPage < TotalPages);

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
    /// </summary>
    /// <param name="items">The list of items in the current page.</param>
    /// <param name="count">The total number of items in the entire collection.</param>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    public PagedList(List<T> items, int count, int currentPage, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PagedList{T}"/> class asynchronously.
    /// </summary>
    /// <param name="source">The source queryable collection.</param>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="pageLimit">The number of items per page.</param>
    /// <returns>A task representing the asynchronous creation of the paged list.</returns>
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageLimit)
    {
        var items = await source.ToListAsync();

        var pagedItems = currentPage != default
            ? source.Skip((currentPage - 1) * pageLimit).Take(pageLimit).ToList()
            : source.ToList();

        return new PagedList<T>(pagedItems, items.Count, currentPage, pageLimit);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PagedList{T}"/> class.
    /// </summary>
    /// <param name="source">The source list of items.</param>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="pageLimit">The number of items per page.</param>
    /// <returns>The created paged list.</returns>
    public static PagedList<T> Create(List<T> source, int currentPage, int pageLimit)
    {
        var items = currentPage != default
            ? source.Skip((currentPage - 1) * pageLimit).Take(pageLimit).ToList()
            : source;

        return new PagedList<T>(items, source.Count, currentPage, pageLimit);
    }
}
