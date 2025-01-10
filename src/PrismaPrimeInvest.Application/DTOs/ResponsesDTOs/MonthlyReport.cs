using System.Xml.Serialization;

namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

public class MonthlyReport
{
    [XmlElement("Cotistas")]
    public required Shareholders Shareholders { get; set; }

    [XmlElement("Resumo")]
    public required Summary Summary { get; set; }
}
