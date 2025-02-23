namespace PrismaPrimeInvest.Application.Filters;

public class FilterFundReport : FilterBase
{
    public string? Type { get; set; }
    public DateTime? ReferenceDate { get; set; }
    public bool? Status { get; set; }
    public Guid? FundId { get; set; }
}
