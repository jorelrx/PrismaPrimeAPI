using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.UserDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;
using PrismaPrimeInvest.Application.Validations.UserValidations;
using PrismaPrimeInvest.Domain.Entities.User;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.UserInterface;

namespace PrismaPrimeInvest.Application.Services.UserServices;

public class UserService(
    IUserRepository repository,
    IMapper mapper
) : BaseService<User, UserDto, CreateUserDto, UpdateUserDto, CreateValidationUser, UpdateValidationUser, FilterUser>(repository, mapper), IUserService {}