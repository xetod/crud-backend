using Microsoft.EntityFrameworkCore;

namespace Crud.Data.Core.PagedLists;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }

    public int TotalPages { get; private set; }

    public int PageSize { get; private set; }

    public int TotalCount { get; private set; }

    public bool HasPrevious => (CurrentPage > 1);

    public bool HasNext => (CurrentPage < TotalPages);

    public PagedList(List<T> items, int count, int currentPage, int pageSize)
    {
        TotalCount = count;

        PageSize = pageSize;

        CurrentPage = currentPage;

        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageLimit)
    {
        var items = await source.ToListAsync();

        var pagedItems = currentPage != default
            ? source.Skip((currentPage - 1) * pageLimit).Take(pageLimit).ToList()
            : source.ToList();

        return new PagedList<T>(pagedItems, items.Count, currentPage, pageLimit);
    }

    public static PagedList<T> Create(List<T> source, int currentPage, int pageLimit)
    {
        var items = currentPage != default
            ? source.Skip((currentPage - 1) * pageLimit).Take(pageLimit).ToList()
            : source;

        return new PagedList<T>(items, source.Count, currentPage, pageLimit);
    }
}
