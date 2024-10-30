using PrismaPrimeInvest.Application.DTOs;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services;

public interface IBaseService<TDto, TCreateDto, TUpdateDto, TFilter>
    where TDto : BaseDto
    where TCreateDto : class
    where TUpdateDto : class
    where TFilter : FilterBase
{
    Task<TDto> GetByIdAsync(Guid id);
    Task<List<TDto>> GetAllAsync(TFilter filter);
    Task<Guid> CreateAsync(TCreateDto dto);
    Task UpdateAsync(Guid id, TUpdateDto dto);
    Task DeleteAsync(Guid id);
}