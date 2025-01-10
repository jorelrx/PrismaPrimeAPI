using System.Text.Json.Serialization;

namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

class ApiAssetReportResponse
{
    [JsonPropertyName("data")]
    public List<ReportResponse>? Data { get; set; }
}