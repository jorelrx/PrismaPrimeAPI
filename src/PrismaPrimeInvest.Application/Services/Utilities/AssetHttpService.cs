using System.Net;
using System.Net.Http.Headers;
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
        _logger.LogInformation("Iniciando método GetDailyPricesByTickerAsync teste");
        HttpClient client = new HttpClient();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://statusinvest.com.br/fiagro/tickerprice");

        request.Headers.Add("accept", "*/*");
        request.Headers.Add("accept-language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
        request.Headers.Add("origin", "https://statusinvest.com.br");
        request.Headers.Add("cookie", "_adasys=28ee49d8-7e51-44b2-b582-cd481f42eb09; hubspotutk=f97d23d2871fe18d1609e17f3b17ccb3; suno_checkout_userid=ff0df9c3-cd4f-4401-a1b0-ea072eebec5b; messagesUtk=5c26a6ad53d941a7932ddeaef06aedf8; _hjSessionUser_1931042=eyJpZCI6Ijc0ODBlNzA0LTk1NTQtNWE1ZC04NjAxLTE0ZGY0OWY2YjUzMiIsImNyZWF0ZWQiOjE3MDMwMDExNzkwMjEsImV4aXN0aW5nIjp0cnVlfQ==; _cc_id=5bc86fbb2fa0fcd4a671a3e54d47a741; FCNEC=%5B%5B%22AKsRol-v1NGUu86AJAq3UpvWgBfCpTQVXy78e-gTGn0uc6Xo9RJEEmb28nYAkNFyNH-ST8J27MPb-wls55fK7P7QB70vMWeznoIToQa4lZGMIHCjerZX96x7zE3t2NONn4td3-oTscrqDZX_XIrim6puyT_N-G1GCQ%3D%3D%22%5D%5D; _ga=GA1.1.713744378.1726659044; _vwo_uuid_v2=DA6ECBC6B724BBE80B459428954BD4473|44a8529d906551a14372f0852ac47492; _gcl_au=1.1.353661899.1729060432; _vwo_uuid=DA6ECBC6B724BBE80B459428954BD4473; _vis_opt_exp_23_combi=1; G_ENABLED_IDPS=google; .StatusInvest=eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SWQiOiI4ODgzMDYiLCJOYW1lIjoiSm9lbCBWaWN0b3IiLCJFbWFpbCI6ImpudWxsLnJ4QGdtYWlsLmNvbSIsIkludGVyZmFjZVR5cGUiOiJXZWIiLCJJcCI6Ijo6ZmZmZjoxMC4xMDAuNC4yMSIsIm5iZiI6MTczMDM4ODUwNSwiZXhwIjoxNzMwNDc0OTA1LCJpYXQiOjE3MzAzODg1MDUsImlzcyI6IlN0YXR1c0ludmVzdCIsImF1ZCI6IlN0YXR1c0ludmVzdCJ9.LYqMeyEUfVNJsdnn3sI3VyS2qHfqxVfGYp-IYnEL6yzqRAJzDJ2g9DY7zRWAU3aYdujSszXoXJ0Ht2D0Gqxleg; __hs_notify_banner_dismiss=true; _vis_opt_s=17%7C; _ga_KHF99X84VQ=deleted; panoramaId_expiry=1736633573235; panoramaId=334e22296ad71f0c6798a7151912a9fb927a1c36fa04c76251d78329461fd8c3; panoramaIdType=panoDevice; __hssrc=1; _clck=uup2p%7C2%7Cfsh%7C0%7C1448; wisepops=%7B%22popups%22%3A%7B%22518984%22%3A%7B%22dc%22%3A4%2C%22d%22%3A1730989577723%2C%22cl%22%3A1%7D%7D%2C%22sub%22%3A0%2C%22ucrn%22%3A62%2C%22cid%22%3A%2252100%22%2C%22v%22%3A4%2C%22bandit%22%3A%7B%22recos%22%3A%7B%7D%7D%7D; cf_clearance=lTcFNsltFCFrDRaFa_ny5BRW92y4CGh9YRVmoJS3.L8-1736575395-1.2.1.1-VrgEKUkL26WPlvss8sLQEerYQ0WrmxIPUhSSYAhjWD8D1H_g8wuc6DZyrt43qdzJ_hLQjGMtxmfRqfk0wX_IfIfAq54ff8n9b2KDDUAZh7i4zTlN6Lf3MiiIrnJkOaB6_LjD3lhUluJmkBul8J5s7FKUCBFq__WGwOJuNDrXWbUzmAloCjxb.m0AvR7519YBDe0cOrX5iHLDsG0RTR1j4tlJg2gMlxYiLRKNGIiXk2dl9nDX_IdtLtl1GLJstvaX0HB1jKSp.6iszZiFmICL89Cz3ff6VczT8.lHO3pjLoA; _ga_69GS6KP6TJ=GS1.1.1736575395.178.0.1736575395.60.0.0; _ga_0Z9P1HTPF9=GS1.1.1736575395.81.0.1736575395.0.0.0; _hjSession_1931042=eyJpZCI6ImNhNjA2ZDBmLTY1ZmUtNDYxZC1iOGVlLTI3MzAxYThkNzZjNSIsImMiOjE3MzY1NzUzOTU0NzQsInMiOjAsInIiOjAsInNiIjowLCJzciI6MCwic2UiOjAsImZzIjowLCJzcCI6MH0=; _ga_KHF99X84VQ=GS1.1.1736575395.81.0.1736575395.0.0.0; wisepops_visitor=%7B%22TfwCNQSfMg%22%3A%2250f8cd8e-c917-43c0-b2b4-5938f56235f9%22%7D; wisepops_visits=%5B%222025-01-11T06%3A03%3A14.551Z%22%2C%222025-01-11T04%3A43%3A23.519Z%22%2C%222025-01-11T04%3A19%3A51.067Z%22%2C%222025-01-11T04%3A18%3A05.012Z%22%2C%222025-01-10T23%3A05%3A59.915Z%22%2C%222025-01-10T22%3A12%3A48.644Z%22%2C%222025-01-10T08%3A25%3A47.259Z%22%2C%222025-01-10T06%3A58%3A43.815Z%22%2C%222025-01-10T06%3A58%3A37.703Z%22%2C%222025-01-10T06%3A58%3A11.116Z%22%5D; wisepops_session=%7B%22arrivalOnSite%22%3A%222025-01-11T06%3A03%3A14.551Z%22%2C%22mtime%22%3A1736575395667%2C%22pageviews%22%3A1%2C%22popups%22%3A%7B%7D%2C%22bars%22%3A%7B%7D%2C%22sticky%22%3A%7B%7D%2C%22countdowns%22%3A%7B%7D%2C%22src%22%3Anull%2C%22utm%22%3A%7B%7D%2C%22testIp%22%3Anull%7D; _clsk=4j798j%7C1736575396354%7C1%7C0%7Ck.clarity.ms%2Fcollect; cto_bundle=FTdUZ19MUWY2bXhvYWdNU1hZcTRNTWFhTzNSOWJWNHQlMkZCWkEyc1plRmVhMmVWdzI4R3d0MHBHMkxFb0NNZWRrSHNxZVpsdlZ0S2pBQjR3RFlIdyUyQjBKT0Q1VFd1M0RleW9NS0c4amdZRCUyRlMlMkZYJTJGU0M4UllCcm56bGI3Z0Y1RUpnS0g4VkNFSmRneVg2VnpyT0ZBSE9Galk3Qk5lN3pVY2FyaFNSNzg3bGJDTSUyRnRJNlpDQ3FmR2xZd1dKendSYWdkeTAzdnZHWHE3eFJiNjJ3VXVRWm13TldsOXhRJTNEJTNE; __hstc=176625274.f97d23d2871fe18d1609e17f3b17ccb3.1690349366121.1736569092063.1736575401926.191; __hssc=176625274.1.1736575401926");
        request.Headers.Add("priority", "u=1, i");
        request.Headers.Add("referer", "https://statusinvest.com.br/fiagros/xpca11");
        request.Headers.Add("sec-ch-ua", "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\"");
        request.Headers.Add("sec-ch-ua-mobile", "?0");
        request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
        request.Headers.Add("sec-fetch-dest", "empty");
        request.Headers.Add("sec-fetch-mode", "cors");
        request.Headers.Add("sec-fetch-site", "same-origin");
        request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");
        request.Headers.Add("x-requested-with", "XMLHttpRequest");

        request.Content = new StringContent("ticker=XPCA11&type=4&currences%5B%5D=1");
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded; charset=UTF-8");

        HttpResponseMessage response = await client.SendAsync(request);
        string responseBody = await response.Content.ReadAsStringAsync();
        _logger.LogInformation($"responseBody: {responseBody}");


        _logger.LogInformation("Iniciando método GetDailyPricesByTickerAsync para o ticker: {Ticker}, tipo de ativo: {TypeAsset}", ticker, typeAsset);

        try
        {
            string urlParamReferrer = typeAsset == 1 ? "fiagros" : "fundos-imobiliarios";
            string urlParamRequest = typeAsset == 1 ? "fiagro" : "fii";

            string requestUrl = $"https://statusinvest.com.br/{urlParamRequest}/tickerprice";

            _logger.LogInformation("Enviando requisição GET para inicializar sessão: {Url}", requestUrl);
            var initialResponse = await Client.GetAsync($"https://statusinvest.com.br/{urlParamReferrer}/{ticker.ToLower()}");

            if (!initialResponse.IsSuccessStatusCode)
            {
                var errorContent = await initialResponse.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro na requisição GET: {initialResponse.StatusCode}, Detalhes: {errorContent}");
            }
            _logger.LogInformation("Requisição GET realizada com sucesso.");

            var postRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            ConfigureRequestHeaders(postRequest, $"https://statusinvest.com.br/{urlParamReferrer}/{ticker.ToLower()}");

            var payload = $"ticker={ticker}&type=4&currences%5B%5D=1";
            postRequest.Content = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");

            _logger.LogInformation("Enviando requisição POST para URL: {Url} com payload: {Payload}", postRequest.RequestUri, payload);
            var postResponse = await Client.SendAsync(postRequest);

            if (!postResponse.IsSuccessStatusCode)
            {
                var errorContent = await postResponse.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro na requisição POST: {postResponse.StatusCode}, Detalhes: {errorContent}");
            }

            var content = await postResponse.Content.ReadAsStringAsync();
            _logger.LogInformation("Resposta recebida com sucesso para o ticker: {Ticker}", ticker);

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
        request.Headers.Add("Sec-CH-UA", "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\"");
        request.Headers.Add("Sec-CH-UA-Mobile", "?0");
        request.Headers.Add("Sec-CH-UA-Platform", "\"Windows\"");
        request.Headers.Add("Sec-Fetch-Dest", "empty");
        request.Headers.Add("Sec-Fetch-Mode", "cors");
        request.Headers.Add("Sec-Fetch-Site", "same-origin");
        request.Headers.Referrer = new Uri(referrer);
    }
}
