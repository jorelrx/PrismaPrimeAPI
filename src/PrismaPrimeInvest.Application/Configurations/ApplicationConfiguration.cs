using PrismaPrimeInvest.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrismaPrimeInvest.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PrismaPrimeInvest.Application.Responses;
using Newtonsoft.Json.Serialization;
using PrismaPrimeInvest.Application.Extensions;

namespace PrismaPrimeInvest.Application.Configurations;
public static class ApplicationConfiguration
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("A conexão com o banco de dados não foi configurada.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("PrismaPrimeInvest.Infra.Data");
            sqlOptions.EnableRetryOnFailure();
        });
        }, ServiceLifetime.Scoped);
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }

    public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection jwtSettings = configuration.GetSection("Jwt");
        string? secretKey = jwtSettings["SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
            throw new InvalidOperationException("A chave secreta do JWT não foi configurada.");
        var key = Encoding.UTF8.GetBytes(secretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            // Customização das mensagens de erro de autenticação e autorização
            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";

                    ApiResponse<string> result = new()
                    {
                        Id = Guid.NewGuid(),
                        Status = HttpStatusCode.Unauthorized,
                        Message = "Authentication failed.",
                        Response = "You are not authorized to access this resource."
                    };

                    return context.Response.WriteAsync(JsonConvert.SerializeObject(result, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    context.Response.ContentType = "application/json";

                    ApiResponse<string> result = new()
                    {
                        Id = Guid.NewGuid(),
                        Status = HttpStatusCode.Forbidden,
                        Message = "Authorization failed.",
                        Response = "You do not have permission to access this resource."
                    };

                    return context.Response.WriteAsync(JsonConvert.SerializeObject(result, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));
                }
            };
        });
    }

    public static void ConfigureHolidaysFile(this IServiceCollection services)
    {
        var holidayFilePath = Path.Combine(AppContext.BaseDirectory, "Resources", "holidays.json");
        DateTimeExtensions.LoadHolidaysFromJson(holidayFilePath);
    }
}