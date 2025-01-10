using System.Xml.Serialization;

namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

public class GeneralData
{
    [XmlElement("NomeFundo")]
    public required string FundName { get; set; }

    [XmlElement("CNPJFundo")]
    public string? FundCNPJ { get; set; }

    [XmlElement("QtdCotasEmitidas")]
    public int IssuedSharesQuantity { get; set; }
}
