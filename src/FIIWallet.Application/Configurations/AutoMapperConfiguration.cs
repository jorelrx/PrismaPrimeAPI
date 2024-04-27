using AutoMapper;
using FIIWallet.Application.DTOs.UserDTOs;
using FIIWallet.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace FIIWallet.Application.Configurations;
public static class AutoMapperConfiguration
{
    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<User, UserDto>().ReverseMap();
            config.CreateMap<User, CreateUserDto>().ReverseMap();
            config.CreateMap<User, UpdateUserDto>().ReverseMap();
        });

        services.AddSingleton(mapperConfig.CreateMapper());
    }
}