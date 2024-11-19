namespace PrismaPrimeInvest.Application.Filters;

public class FilterFundDailyPrice : FilterBase
{
    public DateTime? Date { get; set; }
    public Guid? FundId { get; set; }
}