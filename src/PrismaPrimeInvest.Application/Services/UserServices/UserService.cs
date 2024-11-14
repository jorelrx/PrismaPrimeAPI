using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PrismaPrimeInvest.Application.DTOs.UserDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;
using PrismaPrimeInvest.Application.Validations.UserValidations;
using PrismaPrimeInvest.Domain.Entities.User;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.UserInterface;

namespace PrismaPrimeInvest.Application.Services.UserServices;

public class UserService(
    IUserRepository repository,
    IMapper mapper,
    UserManager<User> userManager
) : BaseService<User, UserDto, CreateUserDto, UpdateUserDto, CreateValidationUser, UpdateValidationUser, FilterUser>(repository, mapper), IUserService 
{
    private readonly UserManager<User> _userManager = userManager;
    
    public override async Task<Guid> CreateAsync(CreateUserDto dto)
    {
        await _createValidator.ValidateAndThrowAsync(dto);
        User entity = _mapper.Map<User>(dto);
        IdentityResult result = await _userManager.CreateAsync(entity, dto.Password);
        
        if (!result.Succeeded)
        {
            throw new CustomIdentityException(result);
        }
        
        return entity.Id;
    }
}
