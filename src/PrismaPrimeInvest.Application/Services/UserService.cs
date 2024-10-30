using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.UserDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services;
using PrismaPrimeInvest.Application.Validations.UserValidation;
using PrismaPrimeInvest.Domain.Entities;
using PrismaPrimeInvest.Domain.Interfaces.Repositories;

namespace PrismaPrimeInvest.Application.Services;

public class UserService(
    IUserRepository repository,
    IMapper mapper
) : BaseService<User, UserDto, CreateUserDto, UpdateUserDto, CreateValidationUser, UpdateValidationUser, FilterUser>(repository, mapper), IUserService {}