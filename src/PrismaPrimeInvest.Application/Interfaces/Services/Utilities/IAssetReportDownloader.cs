using PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

namespace PrismaPrimeInvest.Application.Interfaces.Services.Utilities;

public interface IAssetReportDownloader
{
    Task<string?> GetIdReportByCnpjAsync(string cnpj);
    Task<List<ReportResponse>?> GetReportsByCnpjAsync(string cnpj);
    Task<string?> DownloadXmlAsync(string idReport, string cnpj);
    AssetData? DeserializeXml(string xmlContent);
}