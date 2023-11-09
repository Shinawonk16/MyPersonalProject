using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost("AddCategory")]
    public async Task<IActionResult> AddCategoryAsync([FromForm] CreateCategoryRequestModel model)
    {
        var add = await _categoryService.AddCategoryAsync(model);
        if (add.Status)
        {
            return Ok(add);
        }
        return BadRequest(add);
    }

    [HttpGet("GetCategoryById/{id}")]
    public async Task<IActionResult> GetCategoryByIdAsync([FromRoute] string id)
    {
        var get = await _categoryService.GetById(id);
        if (get.Status)
        {
            return Ok(get);
        }
        return NotFound(get);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllCategoryAsync()
    {
        var getAll = await _categoryService.GetAllCategory();
        if (getAll.Status)
        {
            return Ok(getAll);
        }
        return NotFound(getAll);
    }
    [HttpPut("UpdateCategory{id}")]
    public async Task<IActionResult> UpdateCategoryAsync([FromRoute] string id, [FromForm] UpdateCategoryRequestModel model)
    {
        var update = await _categoryService.UpdateCategoryAsync(id, model);
        if (update.Status)
        {
            return Ok(update);
        }
        return BadRequest(update);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        var get = await _categoryService.DeleteAsync(id);
        if (get.Status)
        {
            return Ok(get);
        }
        return BadRequest(get);
    }

}
