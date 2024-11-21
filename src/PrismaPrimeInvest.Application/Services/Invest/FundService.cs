using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Validations.FundValidations;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;

namespace PrismaPrimeInvest.Application.Services.Invest;

public class FundService(
    IFundRepository repository,
    IMapper mapper
) : BaseService<Fund, FundDto, CreateFundDto, UpdateFundDto, CreateValidationFund, UpdateValidationFund, FilterFund>(repository, mapper), IFundService 
{
    protected override IQueryable<Fund> ApplyFilters(IQueryable<Fund> query, FilterFund filter)
    {
        if (!string.IsNullOrEmpty(filter.Code))
            query = query.Where(f => f.Code == filter.Code);

        return query;
    }

    public async Task<Fund?> GetByCodeAsync(string code)
    {
        var query = _repository.GetAllAsync();
        return await query.FirstOrDefaultAsync(f => f.Code == code);
    }

    public async Task<List<Fund>> GetAllEntitiesAsync(FilterFund filter)
    {
        var query = _repository.GetAllAsync();
        query = ApplyFilters(query, filter);
        var entities = await query.Include(p => p.DailyPrices).ToListAsync();
        return entities;
    }
}