using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.UserDTOs;
using PrismaPrimeInvest.Domain.Entities.User;
using Microsoft.Extensions.DependencyInjection;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Enums;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;
using PrismaPrimeInvest.Application.DTOs.WalletDTOs;

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

            config.CreateMap<Wallet, WalletDto>().ReverseMap();
            config.CreateMap<Wallet, CreateWalletDto>().ReverseMap();
            config.CreateMap<Wallet, UpdateWalletDto>().ReverseMap();

            config.CreateMap<FundDailyPrice, FundDailyPriceDto>().ReverseMap();
            config.CreateMap<FundDailyPrice, CreateFundDailyPriceDto>().ReverseMap();
            config.CreateMap<FundDailyPrice, UpdateFundDailyPriceDto>().ReverseMap();

            config.CreateMap<FundPayment, FundPaymentDto>().ReverseMap();
            config.CreateMap<FundPayment, CreateFundPaymentDto>().ReverseMap();
            config.CreateMap<FundPayment, UpdateFundPaymentDto>().ReverseMap();
            
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