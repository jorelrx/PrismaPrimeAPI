using PrismaPrimeInvest.Application.DTOs.WalletDTOs;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;

public interface IWalletService : IBaseService<WalletDto, CreateWalletDto, UpdateWalletDto, FilterWallet> {}