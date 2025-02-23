namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundReport;

public class FundReportDto : BaseDto
{
    public required new Guid Id { get; set; }
    public int ReportId { get; set; }
    public string? Type { get; set; }
    public string? ReferenceDate { get; set; }
    public bool Status { get; set; }
}
