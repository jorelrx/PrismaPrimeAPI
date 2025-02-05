using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Application.DTOs.WalletDTOs;
using PrismaPrimeInvest.Application.DTOs.WalletUserDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Interfaces.Services.Relationships;
using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;
using PrismaPrimeInvest.Application.Validations.WalletValidations;
using PrismaPrimeInvest.Domain.Entities.Relationships;
using PrismaPrimeInvest.Domain.Entities.User;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Relationships;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.UserInterface;

namespace PrismaPrimeInvest.Application.Services.UserServices;

public class WalletService(
    IWalletRepository repository,
    IMapper mapper,
    IFundService fundService,
    IUserService userService,
    IWalletUserService walletUserService,
    IWalletFundRepository _walletFundRepository,
    IFundPaymentService fundPaymentService
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
    private readonly IFundPaymentService _fundPaymentService = fundPaymentService;
    private readonly IUserService _userService = userService;
    private readonly IWalletUserService _walletUserService = walletUserService;

    public override async Task<List<WalletDto>> GetAllAsync(FilterWallet filter)
    {
        IQueryable<Wallet> query = _repository.GetAllAsync()
            .Include(w => w.CreatedByUser)
            .Include(w => w.WalletFunds)
                .ThenInclude(wf => wf.Fund);

        query = ApplyFilters(query, filter);

        var wallets = await query.ToListAsync();
        return _mapper.Map<List<WalletDto>>(wallets);
    }

    public override async Task<WalletDto> GetByIdAsync(Guid id)
    {
        Wallet? wallet = await _repository.GetAllAsync()
            .Include(w => w.CreatedByUser)
            .Include(w => w.WalletFunds)
                .ThenInclude(wf => wf.Fund)
            .FirstOrDefaultAsync(w => w.Id == id);

        return _mapper.Map<WalletDto>(wallet);
    }
    
    public async Task<Guid> CreateAsync(CreateWalletDto dto, Guid userId)
    {
        await _createValidator.ValidateAndThrowAsync(dto);
        
        User user = await _userService.GetEntityByIdAsync(userId) ?? throw new Exception("User not found.");

        Wallet entity = new()
        {
            CreatedByUserId = user.Id,
            CreatedByUser = user,
            IsPublic = dto.IsPublic ?? false,
            Name = dto.Name ?? "Wallet"
        };

        await _repository.CreateAsync(entity);

        CreateWalletUserDto walletUser = new()
        {
            WalletId = entity.Id,
            UserId = user.Id
        };

        await _walletUserService.CreateAsync(walletUser);

        return entity.Id;
    }

    public async Task<WalletDto?> GetWalletByUserId(Guid userId)
    {
        Wallet? wallet = await _walletRepository.GetWalletByUserId(userId);
        return _mapper.Map<WalletDto>(wallet);
    }

    private async Task<Wallet?> GetEntityByIdAsync(Guid id) => await _walletRepository.GetByIdAsync(id);

    public async Task PurchaseFundAsync(Guid userId, FundPurchaseDto purchaseDto)
    {
        User user = await _userService.GetEntityByIdAsync(userId) ?? throw new Exception("User not found.");

        Wallet wallet = await GetEntityByIdAsync(purchaseDto.WalletId) ?? throw new Exception("Wallet not found for user.");

        if (user.Id != wallet.CreatedByUserId) throw new Exception("User is not the owner of the wallet.");

        if (await _fundService.GetByIdAsync(purchaseDto.FundId) == null) throw new Exception("Fund not found.");

        var walletFund = new WalletFund
        {
            FundId = purchaseDto.FundId,
            WalletId = wallet.Id,
            PurchaseDate = purchaseDto.PurchaseDate,
            PurchasePrice = purchaseDto.PurchasePrice,
            Quantity = purchaseDto.Quantity
        };

        await _walletFundRepository.CreateAsync(walletFund);
    }
}