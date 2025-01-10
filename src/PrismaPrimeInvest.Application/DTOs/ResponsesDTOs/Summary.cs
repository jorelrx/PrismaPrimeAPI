using System.Xml.Serialization;

namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

public class Summary
{
    [XmlElement("Ativo")]
    public double Asset { get; set; }

    [XmlElement("PatrimonioLiquido")]
    public double NetWorth { get; set; }

    [XmlElement("NumCotasEmitidas")]
    public double IssuedSharesCount { get; set; }

    [XmlElement("ValorPatrCotas")]
    public double ShareValue { get; set; }
}
