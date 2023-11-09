using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VerificationController : ControllerBase
{
    private readonly IVerificationService _verificationService;

    public VerificationController(IVerificationService verificationService)
    {
        _verificationService = verificationService;
    }

    [HttpGet("VerifyCode/{code}/{id}")]
    public async Task<IActionResult> VerifyCode([FromRoute] int code, string id)
    {
        var verifycode = await _verificationService.VerifyCode(id, code);
        if (!verifycode.Status)
        {
            return BadRequest(verifycode);
        }
        return Ok(verifycode);
    }

    [HttpPut("UpdateCode/{id}")]
    public async Task<IActionResult> UpdateCodeAsync([FromRoute] string id)
    {
        var code = await _verificationService.UpdateVerificationCodeAsync(id);
        if (!code.Status)
        {
            return BadRequest(code);
        }
        return Ok(code);
    }

    [HttpPut("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequestModel model)
    {
        var reset = await _verificationService.SendForgetPasswordVerificationCode(model);
        if (!reset.Status)
        {
            return BadRequest(reset);
        }
        return Ok(reset);
    }

}
