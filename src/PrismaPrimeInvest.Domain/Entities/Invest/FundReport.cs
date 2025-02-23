using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace PrismaPrimeInvest.Domain.Entities.Invest;

public class FundReport : BaseEntity
{
    public int ReportId { get; set; }
    public string Type { get; set; }
    public string ReferenceDate { get; set; }
    public bool Status { get; set; }

    public Guid FundId { get; set; }
    public required Fund Fund { get; set; }
    
    [NotMapped]
    public DateTime ReferenceDateAsDate
    {
        get
        {
            string[] formats = { "MM/yyyy", "dd/MM/yyyy" };
            if (DateTime.TryParseExact(ReferenceDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            return DateTime.MinValue;
        }
    }
}
