using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundReport;
using PrismaPrimeInvest.Application.Extensions;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Interfaces.Services.Utilities;
using PrismaPrimeInvest.Application.Responses;
using PrismaPrimeInvest.Application.Validations.FundReportValidations;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;

namespace PrismaPrimeInvest.Application.Services.Invest;

public class FundReportService(
    IFundReportRepository repository,
    IFundRepository fundRepository,
    IFundDailyPriceService fundDailyPriceService,
    IAssetReportDownloader assetReportDownloader,
    IMapper mapper
) : BaseService<
    FundReport,
    FundReportDto,
    CreateFundReportDto,
    UpdateFundReportDto,
    CreateFundReportValidator,
    UpdateFundReportValidator,
    FilterFundReport
>(repository, mapper), IFundReportService 
{
    private readonly IFundRepository _fundRepository = fundRepository;
    private readonly IFundDailyPriceService _fundDailyPriceService = fundDailyPriceService;
    private readonly IAssetReportDownloader _assetReportDownloader = assetReportDownloader;

    protected override IQueryable<FundReport> ApplyFilters(IQueryable<FundReport> query, FilterFundReport filter)
    {
        query = base.ApplyFilters(query, filter);

        if (filter.FundId != null)
            query = query.Where(f => f.FundId == filter.FundId);

        return query;
    }

    public override async Task<PagedResult<FundReportDto>> GetAllAsync(FilterFundReport filter)
    {
        var query = _repository.GetAllAsync();
        query = ApplyFilters(query, filter);
        
        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((filter.Page - 1) * (filter.PageSize ?? totalItems))
            .Take(filter.PageSize ?? totalItems)
            .ToListAsync();
        var ordenedEntities = items.OrderByDescending(x => x.ReferenceDateAsDate);

        return new PagedResult<FundReportDto>(_mapper.Map<List<FundReportDto>>(items), totalItems, filter.Page, filter.PageSize ?? totalItems);
    }

    private async Task<FundReport?> GetByReportIdAsync(int id)
    {
        return await _repository.GetAllAsync().Where(f => f.ReportId == id).FirstOrDefaultAsync();
    }

    public async Task SyncByFundIdAsync(Guid fundId)
    {
        var fund = await _fundRepository.GetByIdAsync(fundId) ?? throw new Exception("Fund not found");
        var fundRepostsRequest = await _assetReportDownloader.GetReportsByCnpjAsync(fund.Cnpj);

        if (fundRepostsRequest == null) throw new Exception("Fetching reports returns null");

        var fundPrices = await _fundDailyPriceService.GetAllAsync(new () { FundId = fund.Id, SortBy = "Date", SortDirection = "asc" });
        var oldestDate = fundPrices.Items.First().Date;
        Console.WriteLine("oldestDate: \n" + oldestDate.ToJson());

        var items = fundRepostsRequest.Where(fr => fr.ReferenceDate.ConvertToDateTime() > oldestDate);
        
        foreach (var fundReportrequest in items)
        {
            var fundReport = await GetByReportIdAsync(fundReportrequest.Id);

            if (fundReport == null)
            {
                var documentType = string.IsNullOrEmpty(fundReportrequest.TypeDocument) ? fundReportrequest.DocumentCategory : fundReportrequest.TypeDocument;

                CreateFundReportDto createFundReportDto = new()
                {
                    ReportId = fundReportrequest.Id,
                    FundId = fundId,
                    Type = documentType,
                    ReferenceDate = fundReportrequest.ReferenceDate,
                    Status = fundReportrequest.Status == "AC"
                };

                await CreateAsync(createFundReportDto);
            }
            else
            {
                if (fundReport.Status != (fundReportrequest.Status == "AC"))
                {
                    UpdateFundReportDto updateFundReportDto = new()
                    {
                        Status = fundReportrequest.Status == "AC"
                    };

                    await UpdateAsync(fundReport.Id, updateFundReportDto);
                }
            }
        }
    }
}
