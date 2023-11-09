using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }
    [HttpPost("AddBrand")]
    public async Task<IActionResult> AddBrandCategory([FromForm] CreateBrandsRequestModel model)
    {
        var create = await _brandService.AddBrandAsync(model);
        if (create.Status)
        {
            return Ok(create);
        }
        return BadRequest(create);
    }
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetProduct([FromRoute] string id)
    {
        var get = await _brandService.GetById(id);
        if (get.Status)
        {
            return Ok(get);
        }
        return BadRequest(get);
    }
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteProductAsync([FromRoute] string id)
    {
        var get = await _brandService.DeleteAsync(id);
        if (get.Status)
        {
            return Ok(get);
        }
        return BadRequest(get);
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllProduct()
    {
        var getAll = await _brandService.GetAllBrand();
        if (getAll.Status)
        {
            return Ok(getAll);
        }
        return BadRequest(getAll);
    }

    [HttpPut("UpdateBrand/{id}")]
    public async Task<IActionResult> UpdateBrandAsync([FromRoute] string id, [FromForm] UpdateBrandsRequestModel model)
    {
        var update = await _brandService.UpdateBrandAsync(id, model);
        if (update.Status)
        {
            return Ok(update);
        }
        return BadRequest(update);
    }

}
