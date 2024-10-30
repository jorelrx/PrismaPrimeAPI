using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs.UserDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;

namespace PrismaPrimeInvest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase<UserDto, CreateUserDto, UpdateUserDto, FilterUser>
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService, IMapper mapper)
            : base(userService, mapper) 
        {
            _userService = userService;
        }
    }
}
