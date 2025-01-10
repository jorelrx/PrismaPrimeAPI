
namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

public class EarningsResponse
{
    public string EarningsThisYear { get; set; }
    public string EarningsLastYear { get; set; }
    public string Rendiment { get; set; }
    public bool RendimentIsUp { get; set; }
    public string ProvisionedThisYear { get; set; }
    public string RendimentWithProvisioned { get; set; }
    public bool RendimentWithProvisionedIsUp { get; set; }
    public Helpers Helpers { get; set; }
    public IEnumerable<AssetEarningsModel> AssetEarningsModels { get; set; }
    public IEnumerable<AssetEarningsYearlyModel> AssetEarningsYearlyModels { get; set; }
}