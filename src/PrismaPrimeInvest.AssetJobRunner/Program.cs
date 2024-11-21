using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrismaPrimeInvest.Application.Configurations;
using PrismaPrimeInvest.AssetJobRunner.Services;
using PrismaPrimeInvest.Infra.IoC;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<AssetHttpService>();

builder.Services.AddScoped<StatusInvestService>();
builder.Services.AddScoped<FundDailyPriceSyncService>();

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.ConfigureAutoMapper();

builder.Build().Run();
