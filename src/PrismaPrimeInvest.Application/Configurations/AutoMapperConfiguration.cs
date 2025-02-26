using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using PrismaPrimeInvest.Domain.Enums;
using PrismaPrimeInvest.Domain.Entities.User;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Entities.Relationships;

using PrismaPrimeInvest.Application.DTOs.UserDTOs;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;
using PrismaPrimeInvest.Application.DTOs.WalletDTOs;
using PrismaPrimeInvest.Application.DTOs.WalletUserDTOs;
using PrismaPrimeInvest.Application.DTOs.FundFavoriteDTOs;
using PrismaPrimeInvest.Application.DTOs.WalletFundDTOs;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundReport;

namespace PrismaPrimeInvest.Application.Configurations;
public static class AutoMapperConfiguration
{
    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<User, UserDto>().ReverseMap();
            config.CreateMap<User, CreateUserDto>().ReverseMap()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => HashPassword(src.Password)));
            config.CreateMap<User, UpdateUserDto>().ReverseMap();

            config.CreateMap<Wallet, WalletDto>()
                .ForMember(dest => dest.CreatedByUserName, opt => 
                    opt.MapFrom(src => src.CreatedByUser != null 
                        ? $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}" 
                        : "Desconhecido"))
                .ForMember(dest => dest.TotalInvested, opt => 
                    opt.MapFrom(src => src.WalletFunds.Sum(wf => wf.PurchasePrice * wf.Quantity)))
                .ForMember(dest => dest.TotalCurrentValue, opt => 
                    opt.MapFrom(src => src.WalletFunds.Sum(wf => wf.Fund != null ? wf.Fund.Price * wf.Quantity : 0)));

            config.CreateMap<Wallet, CreateWalletDto>().ReverseMap();
            config.CreateMap<Wallet, UpdateWalletDto>().ReverseMap();

            config.CreateMap<FundDailyPrice, FundDailyPriceDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Fund.Name))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Fund.Code))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Fund.Type.ToString()));

            config.CreateMap<FundDailyPrice, CreateFundDailyPriceDto>().ReverseMap();
            config.CreateMap<FundDailyPrice, UpdateFundDailyPriceDto>().ReverseMap();

            config.CreateMap<FundPayment, FundPaymentDto>().ReverseMap();
            config.CreateMap<FundPayment, CreateFundPaymentDto>().ReverseMap();
            config.CreateMap<FundPayment, UpdateFundPaymentDto>().ReverseMap();

            config.CreateMap<FundFavorite, FundFavoriteDto>().ReverseMap();
            config.CreateMap<FundFavorite, CreateFundFavoriteDto>().ReverseMap();
            config.CreateMap<FundFavorite, UpdateFundFavoriteDto>().ReverseMap();

            config.CreateMap<FundReport, FundReportDto>().ReverseMap();
            config.CreateMap<FundReport, CreateFundReportDto>().ReverseMap();
            config.CreateMap<FundReport, UpdateFundReportDto>().ReverseMap();

            config.CreateMap<WalletUser, WalletUserDto>().ReverseMap();
            config.CreateMap<WalletUser, CreateWalletUserDto>().ReverseMap();
            config.CreateMap<WalletUser, UpdateWalletUserDto>().ReverseMap();

            config.CreateMap<WalletFund, WalletFundDto>()
                .ForMember(dest => dest.FundName, opt =>
                    opt.MapFrom(src => src.Fund != null
                        ? src.Fund.Code
                        : ""))
                .ForMember(dest => dest.TotalAmount, opt =>
                    opt.MapFrom(src => src.PurchasePrice * src.Quantity));

            config.CreateMap<WalletFund, CreateWalletFundDto>().ReverseMap();
            config.CreateMap<WalletFund, UpdateWalletFundDto>().ReverseMap();
            
            config.CreateMap<Fund, FundDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Pvp, opt => opt.MapFrom(src => src.Price / src.NetAssetValuePerShare))
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

    // Método para fazer o hashing da senha
    private static string HashPassword(string password)
    {
        var passwordHasher = new PasswordHasher<User>();
        string hash = passwordHasher.HashPassword(null, password);
        return hash;
    }
}