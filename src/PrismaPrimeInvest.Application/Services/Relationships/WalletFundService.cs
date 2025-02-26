using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Application.DTOs.WalletFundDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Relationships;
using PrismaPrimeInvest.Application.Responses;
using PrismaPrimeInvest.Application.Validations.WalletFundValidations;
using PrismaPrimeInvest.Domain.Entities.Relationships;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Relationships;

namespace PrismaPrimeInvest.Application.Services.Relationships;

public class WalletFundService(
    IWalletFundRepository repository,
    IMapper mapper
) : BaseService<
    WalletFund, 
    WalletFundDto, 
    CreateWalletFundDto, 
    UpdateWalletFundDto, 
    CreateWalletFundValidation, 
    UpdateWalletFundValidation, 
    FilterWalletFund
>(repository, mapper), IWalletFundService 
{
    public override async Task<PagedResult<WalletFundDto>> GetAllAsync(FilterWalletFund filter)
    {
        IQueryable<WalletFund>? query = _repository.GetAllAsync().Include(w => w.Fund);
        query = ApplyFilters(query, filter);
        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((filter.Page - 1) * (filter.PageSize ?? totalItems))
            .Take(filter.PageSize ?? totalItems)
            .ToListAsync();

        return new PagedResult<WalletFundDto>(_mapper.Map<List<WalletFundDto>>(items), totalItems, filter.Page, filter.PageSize ?? totalItems);
    }
}
