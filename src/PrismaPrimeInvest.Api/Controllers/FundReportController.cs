using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundReport;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;

namespace PrismaPrimeInvest.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class FundReportController(
    IFundReportService fundReportService, 
    IMapper mapper
) : ControllerBase<FundReportDto, CreateFundReportDto, UpdateFundReportDto, FilterFundReport>(fundReportService, mapper) 
{}
