using PrismaPrimeInvest.Application.DTOs.WalletDTOs;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;

public interface IWalletService : IBaseService<WalletDto, CreateWalletDto, UpdateWalletDto, FilterWallet> 
{
    Task<Guid> CreateAsync(CreateWalletDto dto, Guid userId);
    Task<WalletDto?> GetWalletByUserId(Guid userId);
    Task PurchaseFundAsync(Guid userId, FundPurchaseDto purchaseDto);
    Task<List<WalletInvestmentAnalysisDto>> GetInvestmentAnalysisAsync(Guid walletId);
}
