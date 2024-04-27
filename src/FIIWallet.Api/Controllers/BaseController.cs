using FIIWallet.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace FIIWallet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController<TDto> : ControllerBase
{
    protected IActionResult HandleResult<TData>(ServiceResponse<TData> response)
    {
        if (response.Success)
        {
            return Ok(response.Data);
        }
        else
        {
            return BadRequest(response.Message);
        }
    }
}