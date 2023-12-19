using Application.Abstractions.IService;
using Application.Auths;
using Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Shopezy.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly IConfiguration _config;
    private readonly IJWTAuthentication _tokenService;

    public RequestController(IRequestService requestService, IConfiguration config, IJWTAuthentication tokenService)
    {
        _requestService = requestService;
        _config = config;
        _tokenService = tokenService;

    }


    [HttpPost("MakeRequest/{managerId}")]
    public async Task<IActionResult> CreateRequestAsync([FromRoute]string managerId, [FromForm] CreateRequestRequestModel model)
    {
        var request = await _requestService.CreateRequestAsync(managerId, model);
        if (!request.Status)
        {
            return BadRequest(request);
        }
        return Ok(request);
    }
    [HttpGet("GetAllRejectedMonthlyRequest/{month}")]
    public async Task<IActionResult> GetAllRejectedMonthlyRequestOnEachProductAsync([FromRoute] int month)
    {
        var request = await _requestService.GetAllRejectedRequestForTheMonthAsync(month);
        if (!request.Status)
        {
            return BadRequest(request);
        }
        return Ok(request);
    }
    [HttpDelete("DeleteRequest/{id}")]
    public async Task<IActionResult> DeleteRequestAsync([FromRoute] string id)
    {
        var request = await _requestService.DeleteAsync(id);
        if (!request.Status)
        {
            return BadRequest(request);
        }
        return Ok(request);
    }

    [HttpGet("GetAllAprovedMonthlyRequest/{month}/{year}")]
    public async Task<IActionResult> GetAllAprovedMonthlyRequestOnEachProductAsync([FromRoute] int month, int year)
    {
        var request = await _requestService.GetAllAprovedRequestForTheMonthAsync(month, year);
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

    [HttpGet("GetAllRejectedYearlyRequest/{year}")]
    public async Task<IActionResult> GetAllUnAprovedYearlyRequestOnEachProductAsync([FromRoute] int year)
    {
        var request = await _requestService.GetAllRejectedReqestForTheYearAsync(year);
        if (!request.Status)
        {
            return BadRequest(request);
        }
        return Ok(request);
    }
    [HttpGet("GetAllPendingRequest")]
    public async Task<IActionResult> GetAllPendingRequestsync()
    {
        var request = await _requestService.GetAllPendingRequest();
        if (!request.Status)
        {
            return BadRequest(request);
        }
        return Ok(request);
    }

    [HttpGet("CalculateCostOfRequestsForTheMonth")]
    public async Task<IActionResult> CalculateCostOfRequestsForTheMonth()
    {
        var cost = await _requestService.CalculateRequestCostForTheMonth();
        if (!cost.Status)
        {
            return BadRequest(cost);
        }
        return Ok(cost);
    }
    [HttpGet("CalculateCostOfRequestsForTheYear")]
    public async Task<IActionResult> CalculateCostOfRequestsForThYear()
    {
        var cost = await _requestService.CalculateRequestCostForThYear();
        if (!cost.Status)
        {
            return BadRequest(cost);
        }
        return Ok(cost);
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

    // [Authorize("Super-Admin")]
    [HttpPut("ApproveRequest/{id}")]
    public async Task<IActionResult> ApproveRequestAsync([FromRoute] string id)
    {
        string token = Request.Headers["Authorization"];
        string extractedToken = token[7..];
        var isValid = JWTAuthentication.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), extractedToken);
        if (!isValid)
        {
            return Unauthorized();
        }
        var expense = await _requestService.ApproveRequestAsync(id);
        if (!expense.Status)
        {
            return BadRequest(expense);
        }
        return Ok(expense);
    }
    [Authorize]
    [HttpPut("RejectRequest/{id}")]
    public async Task<IActionResult> RejectRequestAsync([FromRoute] string id, [FromBody] RejectRequestRequestModel model)
    {
        string token = Request.Headers["Authorization"];
        string extractedToken = token.Substring(7);
        var isValid = JWTAuthentication.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), extractedToken);
        if (!isValid)
        {
            return Unauthorized();
        }
        var expense = await _requestService.RejectRequestAsync(id, model);
        if (!expense.Status)
        {
            return BadRequest(expense);
        }
        return Ok(expense);
    }

    [HttpGet("GetAllAprovedYearlyRequest/{year}")]
    public async Task<IActionResult> GetAllAprovedYearlyRequestOnEachProductAsync([FromRoute] int year)
    {
        var request = await _requestService.GetAllAprovedRequestForTheYear(year);
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
        var request = await _requestService.UpdateRequestAsync(id, model);
        if (!request.Status)
        {
            return BadRequest(request);
        }
        return Ok(request);
    }
}
