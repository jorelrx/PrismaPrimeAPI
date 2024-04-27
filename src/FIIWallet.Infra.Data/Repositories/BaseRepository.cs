using FIIWallet.Domain.Entities;
using FIIWallet.Domain.Repositories;
using FIIWallet.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FIIWallet.Infra.Data.Repositories;
public class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ApplicationDbContext _context = context;
    private readonly DbSet<TEntity> _entity  = context.Set<TEntity>();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _entity .FindAsync(id);
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await _entity.ToListAsync();
    }

    public virtual async Task CreateAsync(TEntity entity)
    {
        await _entity.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        _entity.Remove(entity);
        await _context.SaveChangesAsync();
    }
}