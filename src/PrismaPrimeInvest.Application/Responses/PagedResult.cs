namespace PrismaPrimeInvest.Application.Responses;

public class PagedResult<T>(List<T> items, int totalItems, int page, int pageSize)
{
    public List<T> Items { get; set; } = items;
    public int TotalItems { get; set; } = totalItems;
    public int Page { get; set; } = page;
    public int PageSize { get; set; } = pageSize;
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}
