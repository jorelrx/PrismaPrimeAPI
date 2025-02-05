namespace PrismaPrimeInvest.Application.Filters;

public class FilterFundPayment : FilterBase
{
    public Guid? FundId { get; set; }
    public int? Period { get; set; }
    public DateTime? Date { get; set; }
}