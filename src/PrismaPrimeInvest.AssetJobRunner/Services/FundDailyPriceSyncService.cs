using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Domain.Entities.Invest;

namespace PrismaPrimeInvest.AssetJobRunner.Services;

public class FundDailyPriceSyncService(IFundDailyPriceService dailyPriceService, IFundService fundService)
{
    private readonly IFundDailyPriceService _dailyPriceService = dailyPriceService;
    private readonly IFundService _fundService = fundService;

    public async Task SyncFundDailyPrices(Fund fund, List<CreateFundDailyPriceDto> dailyPrices)
    {
        var filter = new FilterFundDailyPrice { FundId = fund.Id };
        var existingRecords = await _dailyPriceService.GetAllAsync(filter);

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
                    FundId = fund.Id
                });
            }
        }

        // Persistir no banco
        if (createDtos.Count > 0)
        {
            await _dailyPriceService.CreateManyAsync(createDtos);
        }

        if (updateDtos.Count > 0)
        {
            await _dailyPriceService.UpdateManyAsync(updateDtos);
        }
        
        var updateFundDto = new UpdateFundDto
        {
            Id = fund.Id,
            Code = fund.Code,
            Name = fund.Name,
            Type = fund.Type.ToString(),
            Price = dailyPrices.Last().ClosePrice,
            MinPrice = dailyPrices.Min(d => d.MinPrice),
            MaxPrice = dailyPrices.Max(d => d.MaxPrice),
        };

        await _fundService.UpdateAsync(fund.Id, updateFundDto);
    }
}
