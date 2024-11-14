using PrismaPrimeInvest.Domain.Entities;
using PrismaPrimeInvest.Domain.Interfaces.Repositories;
using PrismaPrimeInvest.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Domain.Interfaces.Entities;

namespace PrismaPrimeInvest.Infra.Data.Repositories;

public class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
{
    protected readonly ApplicationDbContext _context = context;
    protected readonly DbSet<TEntity> _entity  = context.Set<TEntity>();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _entity.FindAsync(id);
    }

    public virtual IQueryable<TEntity> GetAllAsync()
    {
        return _entity.AsQueryable();
    }

    public virtual async Task CreateAsync(TEntity entity)
    {
        await _entity.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Entry(entity).Property(e => e.UpdatedAt).IsModified = true;
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        _entity.Remove(entity);
        await _context.SaveChangesAsync();
    }
}