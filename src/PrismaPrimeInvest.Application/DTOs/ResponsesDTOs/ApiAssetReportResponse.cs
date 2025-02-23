using System.Text.Json.Serialization;

namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

public class ApiAssetReportResponse
{
    [JsonPropertyName("data")]
    public List<ReportResponse>? Data { get; set; }
}