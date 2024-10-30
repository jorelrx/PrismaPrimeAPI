using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.UserDTOs;
using PrismaPrimeInvest.Domain.Entities.User;
using Microsoft.Extensions.DependencyInjection;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs;
using PrismaPrimeInvest.Domain.Enums;

namespace PrismaPrimeInvest.Application.Configurations;
public static class AutoMapperConfiguration
{
    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<User, UserDto>().ReverseMap();
            config.CreateMap<User, CreateUserDto>().ReverseMap();
            config.CreateMap<User, UpdateUserDto>().ReverseMap();
            
            config.CreateMap<Fund, FundDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<FundTypeEnum>(src.Type)));
            config.CreateMap<Fund, CreateFundDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<FundTypeEnum>(src.Type)));
            config.CreateMap<Fund, UpdateFundDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<FundTypeEnum>(src.Type)));
        });

        services.AddSingleton(mapperConfig.CreateMapper());
    }
}