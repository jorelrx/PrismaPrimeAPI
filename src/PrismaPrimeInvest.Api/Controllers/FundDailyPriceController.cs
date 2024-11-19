using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Responses;

namespace PrismaPrimeInvest.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FundDailyPriceController(
    IFundDailyPriceService fundDailyPriceService, IMapper mapper
) : ControllerBase<FundDailyPriceDto, CreateFundDailyPriceDto, UpdateFundDailyPriceDto, FilterFundDailyPrice>(fundDailyPriceService, mapper)
{
    [HttpGet]
    [AllowAnonymous]
    public override async Task<IActionResult> GetAllAsync([FromQuery] FilterFundDailyPrice filter)
    {
        var response = new ApiResponse<List<FundDailyPriceDto>>
        {
            Id = Guid.NewGuid(),
            Status = HttpStatusCode.OK,
            Response = await _service.GetAllAsync(filter)
        };
        return Ok(response);
    }
}
