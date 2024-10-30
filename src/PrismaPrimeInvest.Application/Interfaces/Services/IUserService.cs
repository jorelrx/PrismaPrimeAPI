using PrismaPrimeInvest.Application.DTOs.UserDTOs;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services;
public interface IUserService : IBaseService<UserDto, CreateUserDto, UpdateUserDto, FilterUser> {}