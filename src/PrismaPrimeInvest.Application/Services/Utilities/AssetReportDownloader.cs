using System.Net;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;
using PrismaPrimeInvest.Application.Interfaces.Services.Utilities;

namespace PrismaPrimeInvest.Application.Services.Utilities;

public class AssetReportDownloader : IAssetReportDownloader
{
    private static readonly HttpClient Client = new(new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new CookieContainer(),
        AllowAutoRedirect = true
    });

    private const string BaseUrl = "https://fnet.bmfbovespa.com.br";

    public async Task<string?> GetIdReportByCnpjAsync(string cnpj)
    {
        string urlListaRelatorios = $"{BaseUrl}/fnet/publico/abrirGerenciadorDocumentosCVM?cnpjFundo={cnpj}";
        string urlPesquisarDados = $"{BaseUrl}/fnet/publico/pesquisarGerenciadorDocumentosDados?d=1&s=0&l=10&o%5B0%5D%5BdataEntrega%5D=desc&idCategoriaDocumento=0&idTipoDocumento=0&idEspecieDocumento=0&isSession=true&_={DateTimeOffset.Now.ToUnixTimeMilliseconds()}";

        try
        {
            // Access the main page to establish session and cookies
            var initialResponse = await Client.GetAsync(urlListaRelatorios);
            initialResponse.EnsureSuccessStatusCode();

            // Search documents using session cookies
            var request = new HttpRequestMessage(HttpMethod.Get, urlPesquisarDados)
            {
                Headers = { Referrer = new Uri(urlListaRelatorios) }
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize JSON into ApiAssetReportResponse class
            var apiResponse = JsonSerializer.Deserialize<ApiAssetReportResponse>(responseBody);
            var relatorio = apiResponse?.Data?.FirstOrDefault(r => r.TypeDocument?.Trim() == "Informe Mensal Estruturado");

            return relatorio?.Id.ToString() ?? "ID not found for specified document type.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching report ID: {ex.Message}");
            return null;
        }
    }

    public async Task<List<ReportResponse>?> GetReportsByCnpjAsync(string cnpj)
    {
        string urlListaRelatorios = $"{BaseUrl}/fnet/publico/abrirGerenciadorDocumentosCVM?cnpjFundo={cnpj}";
        string urlPesquisarDados = $"{BaseUrl}/fnet/publico/pesquisarGerenciadorDocumentosDados?d=1&s=0&l=100&o%5B0%5D%5BdataEntrega%5D=desc&idCategoriaDocumento=0&idTipoDocumento=0&idEspecieDocumento=0&isSession=true&_={DateTimeOffset.Now.ToUnixTimeMilliseconds()}";

        try
        {
            var initialResponse = await Client.GetAsync(urlListaRelatorios);
            initialResponse.EnsureSuccessStatusCode();

            var request = new HttpRequestMessage(HttpMethod.Get, urlPesquisarDados)
            {
                Headers = { Referrer = new Uri(urlListaRelatorios) }
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiAssetReportResponse>(responseBody);
            
            return apiResponse?.Data;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching report ID: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> DownloadXmlAsync(string idReport, string cnpj)
    {
        string url = $"{BaseUrl}/fnet/publico/downloadDocumento?id={idReport}";
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            ConfigureRequestHeaders(request, cnpj);

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading XML: {ex.Message}");
            return null;
        }
    }

    public AssetData? DeserializeXml(string xmlContent)
    {
        try
        {
            var serializer = new XmlSerializer(typeof(AssetData));
            var settings = new XmlReaderSettings { IgnoreWhitespace = true };

            using var reader = XmlReader.Create(new StringReader(xmlContent), settings);

            var xmlnsOverrides = new XmlSerializerNamespaces();
            xmlnsOverrides.Add(string.Empty, string.Empty);

            return (AssetData)serializer.Deserialize(reader);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deserializing XML: {ex.InnerException?.Message ?? ex.Message}");
            return null;
        }
    }

    private static void ConfigureRequestHeaders(HttpRequestMessage request, string cnpj)
    {
        request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
        request.Headers.Add("Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
        request.Headers.Add("Connection", "keep-alive");
        request.Headers.Add("Referer", $"{BaseUrl}/fnet/publico/abrirGerenciadorDocumentosCVM?cnpjFundo={cnpj}");
        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");
    }
}
