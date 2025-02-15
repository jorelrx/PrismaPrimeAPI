using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Application.DTOs.WalletFundDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Relationships;
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
    public override async Task<List<WalletFundDto>> GetAllAsync(FilterWalletFund filter)
    {
        IQueryable<WalletFund>? query = _repository.GetAllAsync().Include(w => w.Fund);
        query = ApplyFilters(query, filter);
        var entities = await query.ToListAsync();
        return _mapper.Map<List<WalletFundDto>>(entities);
    }
}
