using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
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

        query = query.OrderBy(x => x.Date);

        return query;
    }

    public override async Task<List<FundDailyPriceDto>> GetAllAsync(FilterFundDailyPrice filter)
    {
        var query = _repository.GetAllAsync();
        query = ApplyFilters(query, filter);
        List<FundDailyPrice>? entities = await query.Include(p => p.Fund).Where(p => p.Fund.Id == p.FundId).ToListAsync();
        Console.WriteLine(entities.Count);
        return _mapper.Map<List<FundDailyPriceDto>>(entities);
    }
    
    public async Task SyncFundDailyPrices(Guid fundId, List<CreateFundDailyPriceDto> dailyPrices)
    {
        var filter = new FilterFundDailyPrice { FundId = fundId };
        var existingRecords = await GetAllAsync(filter);

        var createDtos = new List<CreateFundDailyPriceDto>();
        var updateDtos = new List<UpdateFundDailyPriceDto>();

        foreach (var dailyPrice in dailyPrices)
        {
            var existing = existingRecords.FirstOrDefault(r => r.Date == dailyPrice.Date);

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