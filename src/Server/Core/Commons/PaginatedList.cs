using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Server.Core.Commons;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalCount { get; }
    public int PageSize { get; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;


    public PaginatedList(IEnumerable<T> items, int count, PaginatedListFilter paginatedListFilter)
    {
        PageIndex = paginatedListFilter.PageIndex;
        TotalCount = count;
        TotalPages = (int)Math.Ceiling(count / (double)paginatedListFilter.TakeSize);
        PageSize = Math.Min(PageIndex * TotalPages, count);
        AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((paginatedListFilter.PageIndex - 1) * paginatedListFilter.TakeSize).Take(paginatedListFilter.TakeSize).ToListAsync(ct);
        return new PaginatedList<T>(items, count, paginatedListFilter);
    }
}

public class PaginatedListFilter
{
    [DefaultValue(1)]
    [Required]
    [Range(minimum: 1, maximum: int.MaxValue)]
    public int PageIndex { get; set; } = 1;

    [DefaultValue(10)]
    [Required]
    [Range(minimum: 10, maximum: 100)]
    public int TakeSize { get; set; } = 10;
}
