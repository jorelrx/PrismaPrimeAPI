using AutoMapper;
using FIIWallet.Application.DTOs.UserDTOs;
using FIIWallet.Application.Interfaces;
using FIIWallet.Domain.Entities;
using FIIWallet.Domain.Repositories;

namespace FIIWallet.Application.Services;
public class UserService(
    IUserRepository repository,
    IMapper mapper
) : BaseService<User, UserDto, CreateUserDto, UpdateUserDto>(repository, mapper), IUserService {}