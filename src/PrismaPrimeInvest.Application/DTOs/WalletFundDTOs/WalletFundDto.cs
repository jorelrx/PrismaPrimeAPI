namespace PrismaPrimeInvest.Application.DTOs.WalletFundDTOs;

public class WalletFundDto : BaseDto
{
    public DateTime PurchaseDate { get; set; }
    public double PurchasePrice { get; set; }
    public int Quantity { get; set; }
    public double TotalAmount { get; set; }
    public required string FundName { get; set; }
}