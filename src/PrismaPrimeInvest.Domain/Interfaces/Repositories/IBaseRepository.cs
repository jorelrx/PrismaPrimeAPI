using PrismaPrimeInvest.Domain.Interfaces.Entities;

namespace PrismaPrimeInvest.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : IBaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid Id);
    IQueryable<TEntity> GetAllAsync();
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}
