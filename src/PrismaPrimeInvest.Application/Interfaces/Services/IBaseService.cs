using PrismaPrimeInvest.Application.DTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Responses;

namespace PrismaPrimeInvest.Application.Interfaces.Services;

public interface IBaseService<TDto, TCreateDto, TUpdateDto, TFilter>
    where TDto : BaseDto
    where TCreateDto : class
    where TUpdateDto : class
    where TFilter : FilterBase
{
    Task<TDto> GetByIdAsync(Guid id);
    Task<PagedResult<TDto>> GetAllAsync(TFilter filter);
    Task<Guid> CreateAsync(TCreateDto dto);
    Task<List<Guid>> CreateManyAsync(IEnumerable<TCreateDto> dtos);
    Task UpdateAsync(Guid id, TUpdateDto dto);
    Task DeleteAsync(Guid id);
    Task UpdateManyAsync(IEnumerable<TUpdateDto> dtos);
}