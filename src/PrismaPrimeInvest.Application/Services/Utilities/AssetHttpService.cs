using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

namespace PrismaPrimeInvest.Application.Services.Utilities;

public class AssetHttpService
{
    private static readonly HttpClient Client = new(new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new CookieContainer(),
        AllowAutoRedirect = true
    });

    private readonly ILogger<AssetHttpService> _logger;

    public AssetHttpService(ILogger<AssetHttpService> logger)
    {
        _logger = logger;
    }

    public async Task<EarningsResponse?> GetEarningsAsync(string ticker, int typeAsset)
    {
        _logger.LogInformation("Iniciando método GetEarningsAsync para o ticker: {Ticker}, tipo de ativo: {TypeAsset}", ticker, typeAsset);

        try
        {
            string urlParamReferrer = typeAsset == 1 ? "fiagros" : "fundos-imobiliarios";
            string urlParamRequest = typeAsset == 1 ? "fiagro" : "fii";
            string urlParamRequest2 = typeAsset == 1 ? "tickerprovents" : "companytickerprovents";

            string requestUrl = $"https://statusinvest.com.br/{urlParamRequest}/{urlParamRequest2}?ticker={ticker}&chartProventsType=2";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            ConfigureRequestHeaders(request, $"https://statusinvest.com.br/{urlParamReferrer}/{ticker.ToLower()}");

            _logger.LogInformation("Enviando requisição para URL: {Url}", request.RequestUri);
            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Resposta recebida com sucesso para o ticker: {Ticker}", ticker);

            return JsonSerializer.Deserialize<EarningsResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dados de earnings para o ticker: {Ticker}, tipo de ativo: {TypeAsset}", ticker, typeAsset);
            throw;
        }
    }

    public async Task<IEnumerable<DailyPriceResponse>?> GetDailyPricesByTickerAsync(string ticker, int typeAsset)
    {
        _logger.LogInformation("Iniciando método GetDailyPricesByTickerAsync para o ticker: {Ticker}, tipo de ativo: {TypeAsset}", ticker, typeAsset);

        try
        {
            string urlParamReferrer = typeAsset == 1 ? "fiagros" : "fundos-imobiliarios";
            string urlParamRequest = typeAsset == 1 ? "fiagro" : "fii";

            string requestUrl = $"https://statusinvest.com.br/{urlParamRequest}/tickerprice";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            ConfigureRequestHeaders(request, $"https://statusinvest.com.br/{urlParamReferrer}/{ticker.ToLower()}");

            var payload = $"ticker={ticker}&type=4&currences%5B%5D=1";
            request.Content = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");

            _logger.LogInformation("Enviando requisição para URL: {Url} com payload: {Payload}", request.RequestUri, payload);
            var response = await Client.SendAsync(request);
            // response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Resposta recebida com sucesso para o ticker: {Ticker}", ticker);
            _logger.LogInformation($"content : {content}");

            return JsonSerializer.Deserialize<IEnumerable<DailyPriceResponse>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter preços diários para o ticker: {Ticker}, tipo de ativo: {TypeAsset}", ticker, typeAsset);
            throw new Exception($"Erro ao obter detalhes diários para o ticker: {ticker}, tipo de ativo: {typeAsset}, Error: {ex.InnerException?.Message ?? ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<DailyPriceResponse>?> GetDailyDetailsAsync(string ticker, int typeAsset)
    {
        _logger.LogInformation("Iniciando método GetDailyDetailsAsync para o ticker: {Ticker}, tipo de ativo: {TypeAsset}", ticker, typeAsset);

        try
        {
            string urlParamReferrer = typeAsset == 1 ? "fiagros" : "fundos-imobiliarios";
            string urlParamRequest = typeAsset == 1 ? "fiagro" : "fii";

            string requestUrl = $"https://statusinvest.com.br/{urlParamRequest}/tickerprice";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            ConfigureRequestHeaders(request, $"https://statusinvest.com.br/{urlParamReferrer}/{ticker.ToLower()}");

            var payload = $"ticker={ticker}&type=-1&currences%5B%5D=1";
            request.Content = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");

            _logger.LogInformation("Enviando requisição para URL: {Url} com payload: {Payload}", request.RequestUri, payload);
            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Resposta recebida com sucesso para o ticker: {Ticker}", ticker);

            return JsonSerializer.Deserialize<IEnumerable<DailyPriceResponse>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter detalhes diários para o ticker: {Ticker}, tipo de ativo: {TypeAsset}", ticker, typeAsset);
            throw;
        }
    }

    private static void ConfigureRequestHeaders(HttpRequestMessage request, string referrer)
    {
        request.Headers.Add("Accept", "*/*");
        request.Headers.Add("Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");
        request.Headers.Add("X-Requested-With", "XMLHttpRequest");
        request.Headers.Referrer = new Uri(referrer);
    }
}
