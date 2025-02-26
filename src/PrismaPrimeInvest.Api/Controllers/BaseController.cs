using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using PrismaPrimeInvest.Application.DTOs;
using PrismaPrimeInvest.Application.Interfaces.Services;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Responses;

namespace PrismaPrimeInvest.Api.Controllers;

[ApiController]
[Authorize]
public abstract class ControllerBase<TDto, TCreateDto, TUpdateDto, TFilter>(IBaseService<TDto, TCreateDto, TUpdateDto, TFilter> service, IMapper mapper) : ControllerBase
    where TDto : BaseDto
    where TCreateDto : class
    where TUpdateDto : class
    where TFilter : FilterBase, new()
{
    protected readonly IBaseService<TDto, TCreateDto, TUpdateDto, TFilter> _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync([FromBody] TCreateDto dto)
    {
        var id = await _service.CreateAsync(dto);
        var response = new ApiResponse<Guid>
        {
            Id = id,
            Status = HttpStatusCode.Created,
            Response = id
        };
        return CreatedAtAction(nameof(this.GetByIdAsync), new { id }, response);
    }

    [ActionName("GetByIdAsync")]
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var entity = await _service.GetByIdAsync(id);
        var response = new ApiResponse<TDto>
        {
            Id = id,
            Status = HttpStatusCode.OK,
            Response = entity
        };
        return Ok(response);
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAllAsync([FromQuery] TFilter filter)
    {
        var response = new ApiResponse<PagedResult<TDto>>
        {
            Id = Guid.NewGuid(),
            Status = HttpStatusCode.OK,
            Response = await _service.GetAllAsync(filter)
        };
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TUpdateDto dto)
    {
        await _service.UpdateAsync(id, dto);
        var response = new ApiResponse<Guid>
        {
            Id = id,
            Status = HttpStatusCode.NoContent,
            Response = id
        };
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _service.DeleteAsync(id);
        var response = new ApiResponse<Guid>
        {
            Id = id,
            Status = HttpStatusCode.NoContent,
            Response = id
        };
        return NoContent();
    }
}
