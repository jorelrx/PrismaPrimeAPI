namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

public class PriceDetail
{
    public double Price { get; set; }
    public string Date { get; set; }
    public DateTime DateConterted => DateTime.ParseExact(Date, "dd/MM/yy HH:mm", null);
}
