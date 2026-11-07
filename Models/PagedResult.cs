namespace Quản_Lý_KhoHàng.Models;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int PageNumber { get; init; }
    public int TotalPages { get; init; }
    public int TotalItems { get; init; }
}
