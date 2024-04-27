using AutoMapper;
using FIIWallet.Application.Interfaces;
using FIIWallet.Domain.Entities;
using FIIWallet.Domain.Repositories;

namespace FIIWallet.Application.Services;
public class BaseService<TEntity, TDto, TCreateDto, TUpdateDto>(IBaseRepository<TEntity> repository, IMapper mapper) : IBaseService<TDto, TCreateDto, TUpdateDto>
    where TEntity : BaseEntity
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
{
    protected readonly IBaseRepository<TEntity> _repository = repository;
    protected readonly IMapper _mapper = mapper;

    public virtual async Task<TDto> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<List<TDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<List<TDto>>(entities);
    }

    public virtual async Task<Guid> CreateAsync(TCreateDto dto)
    {
        TEntity entity = _mapper.Map<TEntity>(dto);
        await _repository.CreateAsync(entity);
        return entity.Id;
    }

    public virtual async Task UpdateAsync(Guid id, TUpdateDto dto)
    {
        TEntity entity = await _repository.GetByIdAsync(id) ??
            throw new Exception($"Entity with id {id} not found.");

        _mapper.Map(dto, entity);
        await _repository.UpdateAsync(entity);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        TEntity entity = await _repository.GetByIdAsync(id) ?? 
            throw new Exception($"Entity with id {id} not found.");
        await _repository.DeleteAsync(entity);
    }
}
