using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{

    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    [HttpPost("AddRole")]
    public async Task<IActionResult> AddRoleAsync([FromForm] CreateRoleRequestModel model)
    {
        var create = await _roleService.AddRoleAsync(model);
        if (create.Status)
        {
            return Ok(create);
        }
        return BadRequest(create);
    }
    [HttpGet("GetRoleByUserId{userId}")]
    public async Task<IActionResult> GetRoleAsync([FromRoute] string userId)
    {
        var get = await _roleService.GetRoleByUserIdAsync(userId);
        if (get.Status)
        {
            return Ok(get);
        }
        return NotFound(get);
    }
    [HttpGet("GetAllRole")]
    public async Task<IActionResult> GetAllRoleAsync()
    {
        var getAll = await _roleService.GetAllRole();
        if (getAll.Status)
        {
            return Ok(getAll);
        }
        return BadRequest(getAll);
    }
    [HttpPut("UpdateRole{id}")]
    public async Task<IActionResult> UpdateRoleAsync([FromForm] UpdateRoleRequestModel model, [FromRoute] string id)
    {
        var update = await _roleService.UpdateRoleAsync(model, id);
        if (update.Status)
        {
            return Ok(update);
        }
        return BadRequest(update);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        var get = await _roleService.DeleteAsync(id);
        if (get.Status)
        {
            return Ok(get);
        }
        return BadRequest(get);
    }

}
