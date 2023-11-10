using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;

    public RequestController(IRequestService requestService)
    {
        _requestService = requestService;
    }


    [HttpPost("MakeRequest")]
    public async Task<IActionResult> CreateRequestAsync([FromForm]CreateRequestRequestModel model)
    {
        var request = await _requestService.CreateRequestAsync(model);
        if (!request.Status)
        {
            return BadRequest(request);
        }
        return Ok(request);
    }
    [HttpDelete("DeleteRequest/{id}")]
    public async Task<IActionResult> DeleteRequestAsync([FromRoute]string id)
    {
        var request = await _requestService.DeleteAsync(id);
        if (!request.Status)
        {
            return BadRequest(request);
        } 
        return Ok(request);
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllRequestAsync()
    {
        var request = await _requestService.GetAllAsync();
        if (!request.Status)
        {
            return BadRequest(request);
        }
        return Ok(request);
    }
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetRequestByIdAsync([FromRoute] string id)
    {
        var request = await _requestService.GetByIdAsync(id);
        if (!request.Status)
        {
            return BadRequest(request);
        }
        return Ok(request);
    }
    [HttpGet("GetSelectedRequest")]
    public async Task<IActionResult> GetSelectedRequestAsync()
    {
        var request = await _requestService.GetSelectedAsync();
        if (!request.Status)
        {
            return BadRequest(request);
        }
        return Ok(request);
    }
    [HttpPatch("UpdateRequest/{id}")]
    public async Task<IActionResult> UpdateRequestAsync([FromRoute] string id, [FromForm] UpdateRequestRequestModel model)
    {
        var request = await _requestService.UpdateRequestAsync(id,model);
        if (!request.Status)
        {
            return BadRequest(request);
        }
        return Ok(request);
    }
}
