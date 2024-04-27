using FIIWallet.Application.DTOs.UserDTOs;

namespace FIIWallet.Application.Interfaces;
public interface IUserService : IBaseService<UserDto, CreateUserDto, UpdateUserDto> {}