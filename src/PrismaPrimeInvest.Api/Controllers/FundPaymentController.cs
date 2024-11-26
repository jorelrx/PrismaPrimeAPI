using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Responses;

namespace PrismaPrimeInvest.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FundPaymentController(
    IFundPaymentService fundPaymentService, IMapper mapper
) : ControllerBase<FundPaymentDto, CreateFundPaymentDto, UpdateFundPaymentDto, FilterFundPayment>(fundPaymentService, mapper)
{
    [HttpGet]
    [AllowAnonymous]
    public override async Task<IActionResult> GetAllAsync([FromQuery] FilterFundPayment filter)
    {
        var response = new ApiResponse<List<FundPaymentDto>>
        {
            Id = Guid.NewGuid(),
            Status = HttpStatusCode.OK,
            Response = await _service.GetAllAsync(filter)
        };

        return Ok(response);
    }
}
