using System.Xml.Serialization;

namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

public class Shareholders
{
    [XmlAttribute("total")]
    public int Total { get; set; }
}
