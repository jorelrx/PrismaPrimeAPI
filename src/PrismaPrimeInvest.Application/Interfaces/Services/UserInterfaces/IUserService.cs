using PrismaPrimeInvest.Application.DTOs.UserDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Domain.Entities.User;

namespace PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;

public interface IUserService : IBaseService<UserDto, CreateUserDto, UpdateUserDto, FilterUser> 
{
    Task<User?> GetEntityByIdAsync(Guid id);
}