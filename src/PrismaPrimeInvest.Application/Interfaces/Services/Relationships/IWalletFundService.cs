using PrismaPrimeInvest.Application.DTOs.WalletFundDTOs;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services.Relationships;

public interface IWalletFundService : IBaseService<WalletFundDto, CreateWalletFundDto, UpdateWalletFundDto, FilterWalletFund> {}
