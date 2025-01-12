using PrismaPrimeInvest.Application.DTOs.FundFavoriteDTOs;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services.Relationships;

public interface IFundFavoriteService : IBaseService<FundFavoriteDto, CreateFundFavoriteDto, UpdateFundFavoriteDto, FilterFundFavorite> {}