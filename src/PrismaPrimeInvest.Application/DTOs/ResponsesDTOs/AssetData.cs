using System.Xml.Serialization;

namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

[XmlRoot("DadosEconomicoFinanceiros", Namespace = "")]
public class AssetData
{
    [XmlElement("DadosGerais")]
    public required GeneralData GeneralData { get; set; }

    [XmlElement("InformeMensal")]
    public required MonthlyReport MonthlyReport { get; set; }
}
