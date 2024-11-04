using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;

namespace PrismaPrimeInvest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundPaymentController(
        IFundPaymentService fundPaymentService, IMapper mapper
    ) : ControllerBase<FundPaymentDto, CreateFundPaymentDto, UpdateFundPaymentDto, FilterFundPayment>(fundPaymentService, mapper)
    {
        private readonly IFundPaymentService _fundPaymentService = fundPaymentService;
    }
}
