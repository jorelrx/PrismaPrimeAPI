using System.Net.Http.Headers;

namespace PrismaPrimeInvest.AssetJobRunner.Services;

public class StatusInvestService
{
    private readonly HttpClient _httpClientFactory = new HttpClient();

    public async Task<string> PostTickerPriceAsync(string ticker, string url)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Add("accept", "*/*");
        request.Headers.Add("accept-language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
        request.Headers.Add("origin", "https://statusinvest.com.br");
        request.Headers.Add("priority", "u=1, i");
        request.Headers.Add("referer", "https://statusinvest.com.br");
        request.Headers.Add("sec-ch-ua", "\"Chromium\";v=\"130\", \"Google Chrome\";v=\"130\", \"Not?A_Brand\";v=\"99\"");
        request.Headers.Add("sec-ch-ua-mobile", "?0");
        request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
        request.Headers.Add("sec-fetch-dest", "empty");
        request.Headers.Add("sec-fetch-mode", "cors");
        request.Headers.Add("sec-fetch-site", "same-origin");
        request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36");
        request.Headers.Add("x-requested-with", "XMLHttpRequest");

        request.Content = new StringContent($"ticker={ticker}&type=-1&currences%5B%5D=1");
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded; charset=UTF-8");

        HttpResponseMessage response = await _httpClientFactory.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();


        return responseBody;
    }
}
