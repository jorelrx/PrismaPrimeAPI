using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;

namespace PrismaPrimeInvest.AssetJobRunner.Functions;

public class SyncFundReportFunction(ILoggerFactory loggerFactory, IFundService fundService, IFundReportService fundReportService, IMapper mapper)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<SyncFundReportFunction>();
    private readonly IFundService _fundService = fundService;
    private readonly IFundReportService _fundReportService = fundReportService;
    private readonly IMapper _mapper = mapper;

    [Function("SyncFundReportFunction")]
    public async Task Run([TimerTrigger("0 0 10,14,18 * * 1-5")] TimerInfo myTimer)
    {
        _logger.LogInformation($"SyncFundReportFunction function executed at: {DateTime.Now}");

        var funds = await _fundService.GetAllEntitiesAsync(new());

        foreach (var fund in funds)
        {
            await _fundReportService.SyncByFundIdAsync(fund.Id);
        }
    }
}
