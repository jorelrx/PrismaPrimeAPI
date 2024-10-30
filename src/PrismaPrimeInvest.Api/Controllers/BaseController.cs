using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeInvest.Application.DTOs;
using PrismaPrimeInvest.Application.Interfaces.Services;
using PrismaPrimeInvest.Application.Filters;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using PrismaPrimeInvest.Application.Responses;

namespace PrismaPrimeInvest.Api.Controllers
{
    [ApiController]
    public abstract class ControllerBase<TDto, TCreateDto, TUpdateDto, TFilter> : ControllerBase
        where TDto : BaseDto
        where TCreateDto : class
        where TUpdateDto : class
        where TFilter : FilterBase, new()
    {
        private readonly IBaseService<TDto, TCreateDto, TUpdateDto, TFilter> _service;
        private readonly IMapper _mapper;

        protected ControllerBase(IBaseService<TDto, TCreateDto, TUpdateDto, TFilter> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] TCreateDto dto)
        {
            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<Guid>
            {
                Id = id,
                StatusCode = HttpStatusCode.Created,
                Response = id
            };
            return CreatedAtAction(nameof(this.GetByIdAsync), new { id }, response);
        }

        [ActionName("GetByIdAsync")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var user = await _service.GetByIdAsync(id);
            var response = new ApiResponse<TDto>
            {
                Id = id,
                StatusCode = HttpStatusCode.OK,
                Response = user
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] TFilter filter)
        {
            var response = new ApiResponse<List<TDto>>
            {
                Id = Guid.NewGuid(),
                StatusCode = HttpStatusCode.OK,
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
                StatusCode = HttpStatusCode.NoContent,
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
                StatusCode = HttpStatusCode.NoContent,
                Response = id
            };
            return NoContent();
        }
    }
}
