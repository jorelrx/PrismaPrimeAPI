using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.WalletDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;
using PrismaPrimeInvest.Application.Responses;

namespace PrismaPrimeInvest.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalletController(
    IWalletService walletService,
    IAuthService authService,
    IMapper mapper
) : ControllerBase<WalletDto, CreateWalletDto, UpdateWalletDto, FilterWallet>(walletService, mapper) 
{
    private new readonly IWalletService _service = walletService;
    private readonly IAuthService _authService = authService;

    [HttpPost]
    public override async Task<IActionResult> CreateAsync([FromBody] CreateWalletDto dto)
    {
        Guid userId = _authService.GetAuthenticatedUserId();
        Guid id = await _service.CreateAsync(dto, userId);
        ApiResponse<Guid> response = new()
        {
            Id = Guid.NewGuid(),
            Status = HttpStatusCode.Created,
            Response = id,
            Message = "Wallet created successfully."
        };
        
        return CreatedAtAction(nameof(this.GetByIdAsync), new { id }, response);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetWalletByUserId(Guid userId)
    {
        WalletDto entity = await _service.GetByIdAsync(userId);
        var response = new ApiResponse<WalletDto>
        {
            Id = userId,
            Status = HttpStatusCode.OK,
            Response = entity
        };

        return Ok(response);
    }

    [HttpPost("fund/purchase")]
    public async Task<IActionResult> PurchaseFund([FromBody] FundPurchaseDto purchaseDto)
    {
        Guid userId = _authService.GetAuthenticatedUserId();

        try
        {
            await _service.PurchaseFundAsync(userId, purchaseDto);
            return Ok(new { message = "Fund purchased successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
