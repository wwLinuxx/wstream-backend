namespace UzTube.Application.Models;

public class PaginatedList<T>
{
    private PaginatedList(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public IReadOnlyCollection<T> Items { get; }

    public int TotalCount { get; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageSize * PageNumber < TotalCount;

    public static PaginatedList<T> Create(
        IReadOnlyCollection<T> items,
        int totalCount,
        int pageNumber,
        int pageSize)
        => new PaginatedList<T>(items, totalCount, pageNumber, pageSize);
}
