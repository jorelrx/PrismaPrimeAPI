using System.Net;

namespace PrismaPrimeInvest.Application.Responses
{
    public class ApiResponse<T>
    {
        public Guid Id { get; set; }            // Identificador único para a requisição
        public HttpStatusCode StatusCode { get; set; } // Código de status HTTP
        public required T Response { get; set; }         // Dados retornados na resposta
        public string Message { get; set; } = string.Empty; // Mensagem adicional, se necessário
    }
}
