﻿namespace PrismaPrimeInvest.Domain.Entities.Invest;

public class FundDailyPrice : BaseEntity
{
    public DateTime Date { get; set; }
    public double OpenPrice { get; set; }
    public double ClosePrice { get; set; }
    public double MaxPrice { get; set; }
    public double MinPrice { get; set; }

    public Guid FundId { get; set; }
    public required Fund Fund { get; set; }
}
