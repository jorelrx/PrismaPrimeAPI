namespace PrismaPrimeInvest.Application.DTOs.WalletFundDTOs;

public class CreateWalletFundDto
{
    public required Guid WalletId { get; set; }
    public required Guid FundId { get; set; }
    public int Quantity { get; set; }
    public double PurchasePrice { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
}