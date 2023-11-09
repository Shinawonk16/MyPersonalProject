using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManagerController : ControllerBase
{

    private readonly IManagerService _managerService;
        private readonly IConfiguration _config;

        public ManagerController(IManagerService managerService, IConfiguration config)
        {
            _managerService = managerService;
            _config = config;
        }


        [HttpPost("RegisterManager")]
        public async Task<IActionResult> RegisterManagerAsync([FromForm] CreateManagerRequestModel model)
        {
            var add = await _managerService.CreateManagersAsync(model);
            if (add.Status)
            {
                return Ok(add);
            }
            return BadRequest(add);
        }
        [HttpPost("CompleteRegistration")]
        public async Task<IActionResult> CompleteRegistrationAsync([FromForm] CompleteManagerRegistrationRequestModel request)
        {
            var isCompleted = await _managerService.CompleteRegistrationAsync(request);
            if (!isCompleted.Status)
            {
                return BadRequest(isCompleted);
            }
            return Ok(isCompleted);
            
        }

        [HttpGet("GetAllAdmins")]
        public async Task<IActionResult> GetAllManagerAsync()
        {
            var manager = await _managerService.GetSelectedAsync();
            if (!manager.Status)
            {
                return BadRequest(manager);
            }
            return Ok(manager);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateManagerAsync([FromRoute]string id,[FromForm]UpdateManagerRequestModel model)
        {
            var update = await _managerService.UpdateManagerAsync(id,model);
            if (update.Status)
            {
                return Ok(update);
            }
            return BadRequest(update);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetManagerById([FromRoute]string id)
        {
            var manager = await _managerService.GetByIdAsync(id);
            if (!manager.Status)
            {
                return BadRequest(manager);
            }
            return Ok(manager);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]string id)
        {
            var get = await _managerService.DeleteAsync(id);
            if (get.Status)
            {
                return Ok(get);
            }
            return BadRequest(get);
        }
 
}
