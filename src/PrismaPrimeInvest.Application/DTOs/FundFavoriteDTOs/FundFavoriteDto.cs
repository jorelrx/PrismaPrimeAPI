using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;

namespace PrismaPrimeInvest.Application.DTOs.FundFavoriteDTOs;
public class FundFavoriteDto : BaseDto
{
    public required FundDto Fund { get; set; }
}