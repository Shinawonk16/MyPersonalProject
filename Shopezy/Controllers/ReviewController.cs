using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewServices;
    public ReviewController(IReviewService reviewServices)
    {
        _reviewServices = reviewServices;
    }
    [HttpPost("CreateReview/{customerId}")]
    public async Task<IActionResult> CreateAsync([FromForm] CreateReviewRequestModel model, [FromRoute] string customerId)
    {
        var review = await _reviewServices.AddReviewAsync(model, customerId);
        if (!review.Status)
        {
            return BadRequest(review);
        }
        return Ok(review);
    }
    [HttpGet("GetAllReviewByCustomer/{id}")]
    public async Task<IActionResult> GetByCustomer([FromRoute] string id)
    {
        var review = await _reviewServices.GetAllReviewByCustomerIdAsync(id);
        if (!review.Status)
        {
            return BadRequest(review);
        }
        return Ok(review);
    }
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] string id)
    {
        var review = await _reviewServices.GetAllReviewByIdAsync(id);
        if (review.Status)
        {
            return BadRequest(review);
        }
        return Ok(review);
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync()
    {
        var reviews = await _reviewServices.GetAllReviewAsync();
        if (!reviews.Status)
        {
            return BadRequest(reviews);
        }
        return Ok(reviews);
    }
    [HttpGet("GetAllUnseenReview")]
    public async Task<IActionResult> GetAllUnseenReviewAsync()
    {
        var reviews = await _reviewServices.GetAllUnSeenReviewAsync();
        if (!reviews.Status)
        {
            return BadRequest(reviews);
        }
        return Ok(reviews);
    }
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromForm] UpdateReviewRequestModel model)
    {
        var review = await _reviewServices.UpdateReviewAsync(id, model);
        if (!review.Status)
        {
            return BadRequest(review);
        }
        return Ok(review);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        var get = await _reviewServices.DeleteAsync(id);
        if (get.Status)
        {
            return Ok(get);
        }
        return BadRequest(get);
    }

}
