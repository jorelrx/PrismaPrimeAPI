using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.WalletDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;

namespace PrismaPrimeInvest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController(
        IWalletService walletService, 
        IMapper mapper
    ) : ControllerBase<WalletDto, CreateWalletDto, UpdateWalletDto, FilterWallet>(walletService, mapper) {}
}
