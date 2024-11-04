using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;

namespace PrismaPrimeInvest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundDailyPriceController(
        IFundDailyPriceService fundDailyPriceService, IMapper mapper
    ) : ControllerBase<FundDailyPriceDto, CreateFundDailyPriceDto, UpdateFundDailyPriceDto, FilterFundDailyPrice>(fundDailyPriceService, mapper)
    {
        private readonly IFundDailyPriceService _fundDailyPriceService = fundDailyPriceService;
    }
}
