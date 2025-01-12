using PrismaPrimeInvest.Application.DTOs.WalletUserDTOs;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services.Relationships;

public interface IWalletUserService : IBaseService<WalletUserDto, CreateWalletUserDto, UpdateWalletUserDto, FilterWalletUser> {}