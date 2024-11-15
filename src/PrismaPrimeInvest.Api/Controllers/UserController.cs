using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.UserDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;
using PrismaPrimeInvest.Application.Responses;

namespace PrismaPrimeInvest.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(
    IUserService userService, 
    IMapper mapper
) : ControllerBase<UserDto, CreateUserDto, UpdateUserDto, FilterUser>(userService, mapper)
{
    private readonly IUserService _userService = userService;

    [AllowAnonymous]
    [HttpPost]
    public override async Task<IActionResult> CreateAsync([FromBody] CreateUserDto dto)
    {
        Guid id = await _userService.CreateAsync(dto);
        ApiResponse<Guid> response = new()
        {
            Id = id,
            Status = HttpStatusCode.Created,
            Response = id
        };
        
        return CreatedAtAction(nameof(this.GetByIdAsync), new { id }, response);
    }
}
