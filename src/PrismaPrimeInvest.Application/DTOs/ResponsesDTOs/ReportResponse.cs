using System.Text.Json.Serialization;

namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

public class ReportResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("tipoDocumento")]
    public string? TypeDocument { get; set; }

    [JsonPropertyName("categoriaDocumento")]
    public required string DocumentCategory { get; set; }

    [JsonPropertyName("dataReferencia")]
    public required string ReferenceDate { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }
}