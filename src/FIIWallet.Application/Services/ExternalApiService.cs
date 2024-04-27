using System.Net.Http.Json;
using Newtonsoft.Json;

namespace FIIWallet.Application.Services
{
    public class ExternalApiService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<TResponse?> SendRequestAsync<TRequest, TResponse>(string url, HttpMethod method, TRequest requestData)
        {
            try
            {
                var request = new HttpRequestMessage(method, url);
                request.Content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao enviar a requisição para a API externa.", ex);
            }
        }
        public async Task<List<TResponse>> GetListFromExternalApiAsync<TResponse>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<TResponse>>(responseContent) ?? new List<TResponse>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching data from the external API.", ex);
            }
        }
    }
}