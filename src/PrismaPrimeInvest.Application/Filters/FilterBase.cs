namespace PrismaPrimeInvest.Application.Filters;

public abstract class FilterBase
{
    public Guid? Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? OrderBy { get; set; }
    public string? SortDirection { get; set; }
}
