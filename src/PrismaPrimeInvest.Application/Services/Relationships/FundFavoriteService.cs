using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Application.DTOs.FundFavoriteDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Interfaces.Services.Relationships;
using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;
using PrismaPrimeInvest.Application.Validations.FundFavoriteValidations;
using PrismaPrimeInvest.Domain.Entities.Relationships;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Relationships;

namespace PrismaPrimeInvest.Application.Services.Relationships;

public class FundFavoriteService(
    IFundFavoriteRepository repository,
    IFundService fundService,
    IAuthService authService,
    IMapper mapper
) : BaseService<FundFavorite, FundFavoriteDto, CreateFundFavoriteDto, UpdateFundFavoriteDto, CreateValidationFundFavorite, UpdateValidationFundFavorite, FilterFundFavorite>(repository, mapper), IFundFavoriteService 
{
    private readonly IAuthService _authService = authService;
    private readonly IFundService _fundService = fundService;
    
    protected override IQueryable<FundFavorite> ApplyFilters(IQueryable<FundFavorite> query, FilterFundFavorite filter)
    {
        if (!string.IsNullOrEmpty(filter.Code))
            query = query.Where(f => f.Fund.Code == filter.Code);

        return query;
    }
    
    public override async Task<Guid> CreateAsync(CreateFundFavoriteDto dto)
    {
        await _createValidator.ValidateAndThrowAsync(dto);

        Guid userId = _authService.GetAuthenticatedUserId() ?? 
            throw new UnauthorizedAccessException("User is not authenticated.");

        var fundDto = await _fundService.GetByCodeAsync(dto.Code) ?? throw new Exception("Fund not found");
        
        FundFavorite entity = new()
        {
            UserId = userId,
            FundId = fundDto.Id
        };

        await _repository.CreateAsync(entity);

        return entity.Id;
    }

    public override async Task<List<FundFavoriteDto>> GetAllAsync(FilterFundFavorite filter)
    {
        Guid userId = _authService.GetAuthenticatedUserId() ?? 
            throw new UnauthorizedAccessException("User is not authenticated.");

        IQueryable<FundFavorite> query = _repository.GetAllAsync()
                                                    .Where(f => f.UserId == userId)
                                                    .Include(f => f.Fund);

        query = ApplyFilters(query, filter);
        var entities = await query.ToListAsync();

        return _mapper.Map<List<FundFavoriteDto>>(entities);
    }
}
