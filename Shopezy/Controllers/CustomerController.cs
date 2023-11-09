using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{

    private readonly ICustomerService _customerService;
    // private readonly IConfiguration _config;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    [HttpPost("RegisterCustomer")]
    public async Task<IActionResult> RegisterCustomerAsync([FromForm] CreateCustomerRequestModel model)
    {
        var register = await _customerService.CreateCustomerAsync(model);
        if (register.Status)
        {
            return Ok(register);
        }
        return BadRequest(register);
    }
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromForm] UpdateCustomerRequestModel model)
    {
        var update = await _customerService.UpdateCustomerAsync(id, model);
        if (update.Status)
        {
            return Ok(update);
        }
        return BadRequest(update);
    }
    [HttpGet("GetCustomerById/{id}")]
    public async Task<IActionResult> GetCustomerByIdAsync([FromRoute] string id)
    {
        var get = await _customerService.GetByIdAsync(id);
        if (get.Status)
        {
            return Ok(get);
        }
        return BadRequest(get);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCustomerAsync()
    {
        var get = await _customerService.GetAllAsync();
        if (get.Status)
        {
            return Ok(get);
        }
        return BadRequest(get);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        var get = await _customerService.DeleteAsync(id);
        if (get.Status)
        {
            return Ok(get);
        }
        return BadRequest(get);
    }

}
