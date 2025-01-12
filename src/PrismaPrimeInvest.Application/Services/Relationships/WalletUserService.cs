using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.WalletUserDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Relationships;
using PrismaPrimeInvest.Application.Validations.WalletUserValidations;
using PrismaPrimeInvest.Domain.Entities.Relationships;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Relationships;

namespace PrismaPrimeInvest.Application.Services.Relationships;

public class WalletUserService(
    IWalletUserRepository repository,
    IMapper mapper
) : BaseService<
    WalletUser, 
    WalletUserDto, 
    CreateWalletUserDto, 
    UpdateWalletUserDto, 
    CreateValidationWalletUser, 
    UpdateValidationWalletUser, 
    FilterWalletUser
>(repository, mapper), IWalletUserService 
{
}
