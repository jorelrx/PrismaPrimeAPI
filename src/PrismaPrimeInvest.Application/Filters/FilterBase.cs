namespace PrismaPrimeInvest.Application.Filters;

public abstract class FilterBase
{
    public Guid? Id { get; set; }
    public string? Search { get; set; }
    public string[]? SearchFields { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    public int Page { get; set; } = 1;
    public int? PageSize { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
