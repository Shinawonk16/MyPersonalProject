using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LikeController : ControllerBase
{
    private readonly ILikeService _likeService;

    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [HttpPost("CreateLike")]
    [ActionName("Like")]
    public async Task<IActionResult> CreateLikeAsyn(CreateLikeRequestModel model)
    {
        var like = await _likeService.CreateLike(model);
        if (like.Status)
        {
            return Ok(like);
        }
        return StatusCode(400, like);
    }

    [HttpGet("GetLikesByReviewId/{reviewId}")]
    public async Task<IActionResult> GetLikesByReviewIdAsync([FromRoute] string reviewId)
    {
        var likes = await _likeService.GetLikesByReviewIdAsync(reviewId);
        if (likes.Status)
        {
            return Ok(likes);
        }
        return StatusCode(400, likes);
    }

}
