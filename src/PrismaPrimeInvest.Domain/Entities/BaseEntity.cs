using PrismaPrimeInvest.Domain.Interfaces.Entities;

namespace PrismaPrimeInvest.Domain.Entities;

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}