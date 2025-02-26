using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Responses;
using PrismaPrimeInvest.Application.Validations.FundDailyPriceValidations;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;

namespace PrismaPrimeInvest.Application.Services.Invest;

public class FundDailyPriceService(
    IFundDailyPriceRepository repository,
    IMapper mapper
) : BaseService<
    FundDailyPrice, 
    FundDailyPriceDto, 
    CreateFundDailyPriceDto, 
    UpdateFundDailyPriceDto, 
    CreateValidationFundDailyPrice, 
    UpdateValidationFundDailyPrice, 
    FilterFundDailyPrice
>(repository, mapper), IFundDailyPriceService 
{
    protected override IQueryable<FundDailyPrice> ApplyFilters(IQueryable<FundDailyPrice> query, FilterFundDailyPrice filter)
    {
        query = base.ApplyFilters(query, filter);
        
        if (filter.Date.HasValue)
            query = query.Where(x => x.Date.Date == filter.Date.Value.Date);

        if (filter.FundId != null)
            query = query.Where(x => x.FundId == filter.FundId);

        if (filter.Period.HasValue)
        {
            DateTime startDate = filter.Period switch
            {
                0 => DateTime.UtcNow.AddDays(-7),
                1 => DateTime.UtcNow.AddDays(-30),
                2 => DateTime.UtcNow.AddMonths(-6),
                3 => DateTime.UtcNow.AddYears(-1),
                4 => DateTime.UtcNow.AddYears(-5),
                5 => DateTime.UtcNow.AddYears(-10),
                _ => DateTime.MinValue
            };

            query = query.Where(x => x.Date >= startDate && x.Date <= DateTime.UtcNow);
        }

        return query;
    }

    public override async Task<PagedResult<FundDailyPriceDto>> GetAllAsync(FilterFundDailyPrice filter)
    {
        var query = _repository.GetAllAsync();
        query = ApplyFilters(query, filter);
        query = query.Include(p => p.Fund).Where(p => p.Fund.Id == p.FundId);
        
        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((filter.Page - 1) * (filter.PageSize ?? totalItems))
            .Take(filter.PageSize ?? totalItems)
            .ToListAsync();

        return new PagedResult<FundDailyPriceDto>(_mapper.Map<List<FundDailyPriceDto>>(items), totalItems, filter.Page, filter.PageSize ?? totalItems);
    }
    
    public async Task SyncFundDailyPrices(Guid fundId, List<CreateFundDailyPriceDto> dailyPrices)
    {
        var filter = new FilterFundDailyPrice { FundId = fundId };
        var existingRecords = await GetAllAsync(filter);

        var createDtos = new List<CreateFundDailyPriceDto>();
        var updateDtos = new List<UpdateFundDailyPriceDto>();

        foreach (var dailyPrice in dailyPrices)
        {
            var existing = existingRecords.Items.FirstOrDefault(r => r.Date == dailyPrice.Date);

            if (existing != null)
            {
                updateDtos.Add(new UpdateFundDailyPriceDto
                {
                    Id = existing.Id,
                    Name = existing.Name,
                    Code = existing.Code,
                    Type = existing.Type,
                    OpenPrice = dailyPrice.OpenPrice,
                    ClosePrice = dailyPrice.ClosePrice,
                    MaxPrice = dailyPrice.MaxPrice,
                    MinPrice = dailyPrice.MinPrice
                });
            }
            else
            {
                createDtos.Add(new CreateFundDailyPriceDto
                {
                    Date = dailyPrice.Date,
                    OpenPrice = dailyPrice.OpenPrice,
                    ClosePrice = dailyPrice.ClosePrice,
                    MaxPrice = dailyPrice.MaxPrice,
                    MinPrice = dailyPrice.MinPrice,
                    FundId = fundId
                });
            }
        }

        if (createDtos.Count > 0)
        {
            await CreateManyAsync(createDtos);
        }

        if (updateDtos.Count > 0)
        {
            await UpdateManyAsync(updateDtos);
        }
    }
}