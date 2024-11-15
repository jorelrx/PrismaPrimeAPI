using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Responses;

namespace PrismaPrimeInvest.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FundController(
    IFundService fundService, IMapper mapper
) : ControllerBase<FundDto, CreateFundDto, UpdateFundDto, FilterFund>(fundService, mapper)
{
    private readonly IFundService _service = fundService;

    [AllowAnonymous]
    [ActionName("GetByIdAsync")]
    [HttpGet("{id}")]
    public override async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var entity = await _service.GetByIdAsync(id);
        var response = new ApiResponse<FundDto>
        {
            Id = id,
            Status = HttpStatusCode.OK,
            Response = entity
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet]
    public override async Task<IActionResult> GetAllAsync([FromQuery] FilterFund filter)
    {
        var response = new ApiResponse<List<FundDto>>
        {
            Id = Guid.NewGuid(),
            Status = HttpStatusCode.OK,
            Response = await _service.GetAllAsync(filter)
        };
        return Ok(response);
    }
}
