namespace FIIWallet.Application.Interfaces;
public interface IBaseService<TDto, TCreateDto, TUpdateDto>
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
{
    Task<TDto> GetByIdAsync(Guid id);
    Task<List<TDto>> GetAllAsync();
    Task<Guid> CreateAsync(TCreateDto dto);
    Task UpdateAsync(Guid id, TUpdateDto dto);
    Task DeleteAsync(Guid id);
}