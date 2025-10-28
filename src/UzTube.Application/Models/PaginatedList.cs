namespace UzTube.Application.Models;

public record PaginatedList<T>(
    IReadOnlyCollection<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize)
{
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => (long)PageSize * PageNumber < TotalCount;

    public static PaginatedList<T> Create(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int pageSize)
    {
        return new PaginatedList<T>(items, totalCount, pageNumber, pageSize);
    }
}