namespace PrismaPrimeInvest.Application.DTOs.WalletDTOs;

public class FundPurchaseDto
{
    public required Guid FundId { get; set; }
    public int Quantity { get; set; }
    public double PurchasePrice { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
}
