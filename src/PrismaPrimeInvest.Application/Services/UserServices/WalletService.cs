using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.WalletDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;
using PrismaPrimeInvest.Application.Validations.WalletValidations;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Entities.Relationships;
using PrismaPrimeInvest.Domain.Entities.User;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Relationships;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.UserInterface;

namespace PrismaPrimeInvest.Application.Services.UserServices;

public class WalletService(
    IWalletRepository repository,
    IMapper mapper,
    IFundService fundService,
    IWalletFundRepository _walletFundRepository
) : BaseService<
    Wallet, 
    WalletDto, 
    CreateWalletDto, 
    UpdateWalletDto, 
    CreateValidationWallet, 
    UpdateValidationWallet, 
    FilterWallet
>(repository, mapper), IWalletService 
{
    private readonly IWalletRepository _walletRepository = repository;
    private readonly IFundService _fundService = fundService;

    public async Task<WalletDto?> GetWalletByUserId(Guid userId)
    {
        Wallet? wallet = await _walletRepository.GetWalletByUserId(userId);
        return _mapper.Map<WalletDto>(wallet);
    }

    public async Task PurchaseFundAsync(Guid userId, FundPurchaseDto purchaseDto)
    {
        var wallet = await GetWalletByUserId(userId);
        if (wallet == null) throw new Exception("Wallet not found for user.");

        var fund = await _fundService.GetByIdAsync(purchaseDto.FundId);
        if (fund == null) throw new Exception("Fund not found.");

        var walletFund = new WalletFund
        {
            FundId = purchaseDto.FundId,
            WalletId = wallet.Id,
            PurchaseDate = purchaseDto.PurchaseDate,
            PurchasePrice = purchaseDto.PurchasePrice,
            Quantity = purchaseDto.Quantity,
            Wallet = _mapper.Map<Wallet>(wallet),
            Fund = _mapper.Map<Fund>(fund)
        };

        await _walletFundRepository.CreateAsync(walletFund);
    }
}