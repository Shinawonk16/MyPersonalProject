using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userServices;
    private readonly IConfiguration _config;

    public UserController(IUserService userServices, IConfiguration config)
    {
        _userServices = userServices;
        _config = config;
    }
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequsetModel model)
    {
        var log = await _userServices.LoginUserAsync(model);
        if (log.Status)
        {
            return Ok(log);
        }
        return BadRequest(log);
    }
    [HttpGet("GetUsersByRole/{role}")]
    public async Task<IActionResult> GetUserByRole([FromRoute] string role)
    {
        var getRole = await _userServices.GetUsersByRoleAsync(role);
        if (getRole != null)
        {
            return Ok(getRole);
        }
        return BadRequest(getRole);
    }
    [HttpGet("GetUserByToken")]
    public async Task<IActionResult> GetUserByTokenAsync([FromQuery] string token)
    {
        var user = await _userServices.GetUserByTokenAsync(token);
        if (!user.Status)
        {
            return BadRequest(user);
        }
        return Ok(user);
    }

}
