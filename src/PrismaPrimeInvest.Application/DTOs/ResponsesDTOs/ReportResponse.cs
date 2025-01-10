using System.Text.Json.Serialization;

namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

class ReportResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("tipoDocumento")]
    public required string TypeDocument { get; set; }
}