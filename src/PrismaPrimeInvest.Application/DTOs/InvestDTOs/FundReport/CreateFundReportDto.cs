namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundReport
{
    public class CreateFundReportDto
    {
        public int ReportId { get; set; }
        public required string Type { get; set; }
        public required string ReferenceDate { get; set; }
        public bool Status { get; set; }
        public Guid FundId { get; set; }
    }
}
