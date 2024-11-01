using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;

namespace PrismaPrimeInvest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundController(
        IFundService fundService, IMapper mapper
    ) : ControllerBase<FundDto, CreateFundDto, UpdateFundDto, FilterFund>(fundService, mapper)
    {
        private readonly IFundService _fundService = fundService;
    }
}
