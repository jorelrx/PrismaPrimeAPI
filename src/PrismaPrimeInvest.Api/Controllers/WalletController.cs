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
    IMapper mapper
) : ControllerBase<WalletDto, CreateWalletDto, UpdateWalletDto, FilterWallet>(walletService, mapper) 
{
    private readonly IWalletService _service = walletService;

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetWalletByUserId(Guid userId)
    {
        WalletDto entity = await _service.GetByIdAsync(userId);
        var response = new ApiResponse<WalletDto>
        {
            Id = userId,
            StatusCode = HttpStatusCode.OK,
            Response = entity
        };

        return Ok(response);
    }

    [HttpPost("fund/purchase")]
    public async Task<IActionResult> PurchaseFund([FromBody] FundPurchaseDto purchaseDto)
    {
        // O UserId poderia vir do token JWT ou ser passado na requisição
        Guid userId = new("3fa85f64-5717-4562-b3fc-2c963f66afa6");

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
