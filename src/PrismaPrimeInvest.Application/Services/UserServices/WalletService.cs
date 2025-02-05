using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.DTOs.WalletDTOs;
using PrismaPrimeInvest.Application.DTOs.WalletUserDTOs;
using PrismaPrimeInvest.Application.Extensions;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Interfaces.Services.Relationships;
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
    IUserService userService,
    IWalletUserService walletUserService,
    IWalletFundRepository _walletFundRepository,
    IFundDailyPriceService fundDailyPriceService
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
    private readonly IFundDailyPriceService _fundDailyPriceService = fundDailyPriceService;
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

    public async Task<List<WalletInvestmentAnalysisDto>> GetInvestmentAnalysisAsync(Guid walletId)
    {
        var wallet = await _repository.GetAllAsync()
            .Include(w => w.WalletFunds)
                .ThenInclude(wf => wf.Fund)
                .ThenInclude(wp => wp.Payments)
            .Where(w => w.Id == walletId)
            .FirstOrDefaultAsync() ?? throw new Exception("Wallet not found");
            
        var transactions = wallet.WalletFunds
            .OrderBy(wf => wf.PurchaseDate)
            .ToList();

        if (transactions.Count == 0)
            return [];

        List<WalletInvestmentAnalysisDto> analysisList = [];
        double totalInvested = 0;
        double totalDividends = 0;
        
        DateTime startDate = transactions.First().PurchaseDate;
        DateTime currentDate = new(startDate.Year, startDate.Month, 1);

        while (currentDate <= DateTime.UtcNow)
        {
            WalletInvestmentAnalysisDto newWalletInvestmentAnalysisDto = new()
            {
                Date = currentDate,
                InvestedInMonth = 0,
                TotalCurrentValue = 0,
                TotalGrossInvested = 0,
                TotalDividends = 0,
                MonthlyEarnings = 0
            };

            foreach (WalletFund transaction in transactions.Where(t => t.PurchaseDate.Year == currentDate.Year && t.PurchaseDate.Month == currentDate.Month))
            {
                newWalletInvestmentAnalysisDto.InvestedInMonth += transaction.PurchasePrice * transaction.Quantity;
            }

            totalInvested += newWalletInvestmentAnalysisDto.InvestedInMonth;
            newWalletInvestmentAnalysisDto.TotalGrossInvested = totalInvested;

            List<Fund?> funds = [.. transactions.Select(t => t.Fund).Distinct()];

            foreach (var fund in funds)
            {
                if (fund != null)
                {
                    if (fund.Payments != null)
                    {
                        var payment = fund.Payments.FirstOrDefault(p =>
                            p.PaymentDate.Year == currentDate.Year &&
                            p.PaymentDate.Month == currentDate.Month
                        );

                        if (payment != null)
                        {
                            foreach (var transaction in transactions.Where(t => t.PurchaseDate <= currentDate && t.FundId == fund.Id))
                            {
                                newWalletInvestmentAnalysisDto.MonthlyEarnings += transaction.Quantity * payment.Dividend;
                            }
                        }
                    }
                    
                    DateTime dateTimeNow = DateTime.UtcNow;
                    FilterFundDailyPrice filterFundDailyPrice = new()
                    {
                        Date = currentDate.Month == dateTimeNow.Month && currentDate.Year == dateTimeNow.Year
                            ? dateTimeNow.GetLastBusinessDay()
                            : currentDate.GetLastBusinessDayOfMonth()
                    };
                    
                    List<FundDailyPriceDto> fundDailyPrices = await _fundDailyPriceService.GetAllAsync(filterFundDailyPrice);

                    FundDailyPriceDto? dailyPriceAtual = fundDailyPrices.FirstOrDefault();
                    if (dailyPriceAtual != null)
                    {
                        foreach (var transaction in transactions.Where(t => t.PurchaseDate <= filterFundDailyPrice.Date && t.FundId == fund.Id))
                        {
                            newWalletInvestmentAnalysisDto.TotalCurrentValue += transaction.Quantity * dailyPriceAtual.ClosePrice;
                        }
                    }
                }
            }

            totalDividends += newWalletInvestmentAnalysisDto.MonthlyEarnings;
            newWalletInvestmentAnalysisDto.TotalDividends = totalDividends;

            analysisList.Add(newWalletInvestmentAnalysisDto);

            currentDate = currentDate.AddMonths(1);
        }

        return analysisList;
    }
}