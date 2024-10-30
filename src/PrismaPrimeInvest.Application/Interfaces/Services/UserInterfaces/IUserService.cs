using PrismaPrimeInvest.Application.DTOs.UserDTOs;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;

public interface IUserService : IBaseService<UserDto, CreateUserDto, UpdateUserDto, FilterUser> {}