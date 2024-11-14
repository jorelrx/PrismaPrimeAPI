using System.Net;

namespace PrismaPrimeInvest.Application.Responses;

public class ApiResponse<T>
{
    public Guid Id { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public required T? Response { get; set; }
    public string Message { get; set; } = string.Empty;
}
