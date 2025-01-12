using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.FundFavoriteDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Relationships;

namespace PrismaPrimeInvest.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FundFavoriteController(
    IFundFavoriteService fundFavoriteService, 
    IMapper mapper
) : ControllerBase<FundFavoriteDto, CreateFundFavoriteDto, UpdateFundFavoriteDto, FilterFundFavorite>(fundFavoriteService, mapper) 
{
}
