namespace UzTube.Application.Models;

public record PaginatedList<T>
{
    private PaginatedList() { }

    private PaginatedList(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public IReadOnlyCollection<T> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => (long)PageSize * PageNumber < TotalCount;

    public static PaginatedList<T> Create(
        IReadOnlyCollection<T> values,
        int totalCount,
        int pageNumber,
        int pageSize)
        => new(values, totalCount, pageNumber, pageSize);
}