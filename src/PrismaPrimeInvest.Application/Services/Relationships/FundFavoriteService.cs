using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.FundFavoriteDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Relationships;
using PrismaPrimeInvest.Application.Validations.FundFavoriteValidations;
using PrismaPrimeInvest.Domain.Entities.Relationships;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Relationships;

namespace PrismaPrimeInvest.Application.Services.Relationships;

public class FundFavoriteService(
    IFundFavoriteRepository repository,
    IMapper mapper
) : BaseService<FundFavorite, FundFavoriteDto, CreateFundFavoriteDto, UpdateFundFavoriteDto, CreateValidationFundFavorite, UpdateValidationFundFavorite, FilterFundFavorite>(repository, mapper), IFundFavoriteService 
{
}
