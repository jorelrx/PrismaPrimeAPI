using FIIWallet.Application.Common;
using FIIWallet.Application.DTOs.UserDTOs;
using FIIWallet.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIIWallet.Api.Controllers;

public class UserController : BaseController<UserDto>
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var response = await _userService.GetByIdAsync(id);
        return Ok(response); 
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var response = await _userService.GetAllAsync();
        return Ok(response); 
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
    {
        var response = await _userService.CreateAsync(createUserDto);
        return Ok(response); 
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto updateUserDto)
    {
        await _userService.UpdateAsync(id, updateUserDto);
        return Ok(); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _userService.DeleteAsync(id);
        return Ok(); 
    }
}