using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.WalletDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;
using PrismaPrimeInvest.Application.Validations.WalletValidations;
using PrismaPrimeInvest.Domain.Entities.User;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.UserInterface;

namespace PrismaPrimeInvest.Application.Services.UserServices;

public class WalletService(
    IWalletRepository repository,
    IMapper mapper
) : BaseService<
    Wallet, 
    WalletDto, 
    CreateWalletDto, 
    UpdateWalletDto, 
    CreateValidationWallet, 
    UpdateValidationWallet, 
    FilterWallet
>(repository, mapper), IWalletService {}