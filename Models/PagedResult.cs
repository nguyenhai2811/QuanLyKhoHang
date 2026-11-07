namespace Quản_Lý_KhoHàng.Models;

public interface IPagedResult
{
    int PageNumber { get; }
    int TotalPages { get; }
    int TotalItems { get; }
}

public sealed class PagedResult<T> : IPagedResult
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int PageNumber { get; init; }
    public int TotalPages { get; init; }
    public int TotalItems { get; init; }
}
