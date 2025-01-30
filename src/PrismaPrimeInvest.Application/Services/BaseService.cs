using AutoMapper;
using PrismaPrimeInvest.Application.Interfaces.Services;
using PrismaPrimeInvest.Domain.Interfaces.Repositories;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Domain.Interfaces.Entities;
using PrismaPrimeInvest.Application.Extensions;

namespace PrismaPrimeInvest.Application.Services
{
    public abstract class BaseService<TEntity, TDto, TCreateDto, TUpdateDto, TCreateValidation, TUpdateValidation, TFilter> (
        IBaseRepository<TEntity> repository, 
        IMapper mapper
    ) : IBaseService<TDto, TCreateDto, TUpdateDto, TFilter>
        where TEntity : class, IBaseEntity
        where TDto : BaseDto
        where TCreateDto : class
        where TUpdateDto : class
        where TCreateValidation : AbstractValidator<TCreateDto>, new()
        where TUpdateValidation : AbstractValidator<TUpdateDto>, new()
        where TFilter : FilterBase, new()
    {
        protected readonly IBaseRepository<TEntity> _repository = repository;
        protected readonly IMapper _mapper = mapper;
        protected readonly TCreateValidation _createValidator = new TCreateValidation();
        protected readonly TUpdateValidation _updateValidator = new TUpdateValidation();

        protected virtual IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> query, TFilter filter)
        {
            // Aplicar filtros base
            if (filter.CreatedAt.HasValue)
                query = query.Where(e => e.CreatedAt.Date == filter.CreatedAt.Value.Date);

            if (filter.UpdatedAt.HasValue)
                query = query.Where(e => e.UpdatedAt.Date == filter.UpdatedAt.Value.Date);

            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                query = query.OrderByDynamic(filter.OrderBy, filter.SortDirection ?? "asc");
            }

            return query;
        }

        public virtual async Task<TDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<List<TDto>> GetAllAsync(TFilter filter)
        {
            var query = _repository.GetAllAsync();
            query = ApplyFilters(query, filter);
            var entities = await query.ToListAsync();
            return _mapper.Map<List<TDto>>(entities);
        }

        public virtual async Task<Guid> CreateAsync(TCreateDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            TEntity entity = _mapper.Map<TEntity>(dto);
            await _repository.CreateAsync(entity);
            return entity.Id;
        }

        public virtual async Task<List<Guid>> CreateManyAsync(IEnumerable<TCreateDto> dtos)
        {
            var entities = _mapper.Map<List<TEntity>>(dtos);
            await _repository.CreateManyAsync(entities);
            return entities.Select(e => e.Id).ToList();
        }

        public virtual async Task UpdateAsync(Guid id, TUpdateDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            TEntity entity = await _repository.GetByIdAsync(id) ??
                throw new Exception($"Entity with id {id} not found.");

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
        }

        public virtual async Task UpdateManyAsync(IEnumerable<TUpdateDto> dtos)
        {
            foreach (var dto in dtos)
            {
                await _updateValidator.ValidateAndThrowAsync(dto);
            }

            var entities = _mapper.Map<IEnumerable<TEntity>>(dtos);
            await _repository.UpdateManyAsync(entities);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            TEntity entity = await _repository.GetByIdAsync(id) ??
                throw new Exception($"Entity with id {id} not found.");
            await _repository.DeleteAsync(entity);
        }
    }
}
