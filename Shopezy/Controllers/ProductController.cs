using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProductAsync([FromForm] AddProductRequestModel model)
    {
        var add = await _productService.CreateProductAsync(model);
        if (add.Status)
        {
            return Ok(add);
        }
        return BadRequest(add);
    }
    [HttpGet("GetProductById/{id}")]
    public async Task<IActionResult> GetProductAsync([FromRoute] string id)
    {
        var get = await _productService.GetAsync(id);
        if (get.Status)
        {
            return Ok(get);
        }
        return NotFound(get);
    }
    [HttpGet("GetProductByPrice/{price}")]
    public async Task<IActionResult> GetProductByPriceAsync([FromRoute] decimal price)
    {
        var get = await _productService.GetProductsByPriceAsync(price);
        if (get.Status)
        {
            return Ok(get);
        }
        return NotFound(get);
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllProduct()
    {
        var getAll = await _productService.GetAllAsync();
        if (getAll != null)
        {
            return Ok(getAll);
        }
        return BadRequest(getAll);
    }
    [HttpGet("GetProductByCategoryId/{categoryId}")]
    public async Task<IActionResult> GetProductByCategoryIdAsync(string categoryId)
    {
        var get = await _productService.GetByProductCategoryAsync(categoryId);
        if (get.Status)
        {
            return Ok(get);
        }
        return NotFound(get);
    }
    [HttpPut("UpdateProduct/{id}")]
    public async Task<IActionResult> UpdateProductAsync([FromRoute] string id, [FromForm] UpdateProductRequestModel model)
    {
        var update = await _productService.UpdateProductAsync(id, model);
        if (update.Status)
        {
            return Ok(update);
        }
        return BadRequest(update);
    }

    [HttpGet("GetAvailableProduct")]
    public async Task<IActionResult> GetAvailableProductAsync()
    {
        var get = await _productService.GetAllAvailableProduct();
        if (get.Status)
        {
            return Ok(get);
        }
        return NotFound(get);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        var get = await _productService.DeleteAsync(id);
        if (get.Status)
        {
            return Ok(get);
        }
        return BadRequest(get);
    }

}
