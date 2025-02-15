using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.WalletFundDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Relationships;

namespace PrismaPrimeInvest.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalletFundController(
    IWalletFundService walletFundService, 
    IMapper mapper
) : ControllerBase<WalletFundDto, CreateWalletFundDto, UpdateWalletFundDto, FilterWalletFund>(walletFundService, mapper) 
{
    [HttpGet]
    [AllowAnonymous]
    public override async Task<IActionResult> GetAllAsync(FilterWalletFund filter)
    {
        return await base.GetAllAsync(filter);
    }
}
