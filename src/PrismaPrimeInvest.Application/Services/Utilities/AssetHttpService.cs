using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

namespace PrismaPrimeInvest.Application.Services.Utilities;

public class AssetHttpService(HttpClient httpClient, ILogger<AssetHttpService> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<AssetHttpService> _logger = logger;

    public async Task<EarningsResponse?> GetEarningsAsync(string ticker, int typeAsset)
    {
        _logger.LogInformation("Iniciando método GetEarningsAsync para o ticker: {Ticker}, tipo de ativo: {TypeAsset}", ticker, typeAsset);
        try
        {
            string urlParamReferrer = typeAsset == 1 ? "fiagros" : "fundos-imobiliarios";
            string urlParamRequest = typeAsset == 1 ? "fiagro" : "fii";
            string urlParamRequest2 = typeAsset == 1 ? "tickerprovents" : "companytickerprovents";

            var request = new HttpRequestMessage(HttpMethod.Get, 
                $"https://statusinvest.com.br/{urlParamRequest}/{urlParamRequest2}?ticker={ticker}&chartProventsType=2");

            AddCommonHeaders(request);
            request.Headers.Referrer = new Uri($"https://statusinvest.com.br/{urlParamReferrer}/{ticker.ToLower()}");

            _logger.LogDebug("Enviando requisição para URL: {Url}", request.RequestUri);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogDebug("Resposta recebida com sucesso para o ticker: {Ticker}", ticker);

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

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://statusinvest.com.br/{urlParamRequest}/tickerprice");
            AddCommonHeaders(request);
            request.Headers.Referrer = new Uri($"https://statusinvest.com.br/{urlParamReferrer}/{ticker.ToLower()}");

            var payload = $"ticker={ticker}&type=4&currences%5B%5D=1";
            request.Content = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");

            _logger.LogDebug("Enviando requisição para URL: {Url} com payload: {Payload}", request.RequestUri, payload);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogDebug("Resposta recebida com sucesso para o ticker: {Ticker}", ticker);

            return JsonSerializer.Deserialize<IEnumerable<DailyPriceResponse>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter preços diários para o ticker: {Ticker}, tipo de ativo: {TypeAsset}", ticker, typeAsset);
            throw new Exception($"Erro ao obter preços diários para o ticker: {ticker}, tipo de ativo: {typeAsset}, Error: {ex.InnerException?.Message ?? ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<DailyPriceResponse>?> GetDailyDetailsAsync(string ticker, int typeAsset)
    {
        _logger.LogInformation("Iniciando método GetDailyDetailsAsync para o ticker: {Ticker}, tipo de ativo: {TypeAsset}", ticker, typeAsset);
        try
        {
            string urlParamReferrer = typeAsset == 1 ? "fiagros" : "fundos-imobiliarios";
            string urlParamRequest = typeAsset == 1 ? "fiagro" : "fii";

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://statusinvest.com.br/{urlParamRequest}/tickerprice");
            AddCommonHeaders(request);
            request.Headers.Referrer = new Uri($"https://statusinvest.com.br/{urlParamReferrer}/{ticker.ToLower()}");

            var payload = $"ticker={ticker}&type=-1&currences%5B%5D=1";
            request.Content = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");

            _logger.LogDebug("Enviando requisição para URL: {Url} com payload: {Payload}", request.RequestUri, payload);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogDebug("Resposta recebida com sucesso para o ticker: {Ticker}", ticker);

            return JsonSerializer.Deserialize<IEnumerable<DailyPriceResponse>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter detalhes diários para o ticker: {Ticker}, tipo de ativo: {TypeAsset}", ticker, typeAsset);
            throw new Exception($"Erro ao obter detalhes diários para o ticker: {ticker}, tipo de ativo: {typeAsset}, Error: {ex.InnerException?.Message ?? ex.Message}", ex);
        }
    }

    private static void AddCommonHeaders(HttpRequestMessage request)
    {
        request.Headers.Add("accept", "*/*");
        request.Headers.Add("accept-language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
        request.Headers.Add("sec-ch-ua", "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\"");
        request.Headers.Add("sec-ch-ua-mobile", "?0");
        request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
        request.Headers.Add("sec-fetch-dest", "empty");
        request.Headers.Add("sec-fetch-mode", "cors");
        request.Headers.Add("sec-fetch-site", "same-origin");
        request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");
        request.Headers.Add("x-requested-with", "XMLHttpRequest");
    }
}
