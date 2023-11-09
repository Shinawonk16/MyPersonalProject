using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Dto;
using Domain.Entities;

namespace Application.Services;

public class LIkeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly ICustomerRepository _customerRepository;

    public LIkeService(ILikeRepository likeRepository, IReviewRepository reviewRepository, ICustomerRepository customerRepository)
    {
        _likeRepository = likeRepository;
        _reviewRepository = reviewRepository;
        _customerRepository = customerRepository;
    }

    public async Task<BaseResponse<LikeDto>> CreateLike(CreateLikeRequestModel model)
    {
        var customer = await _customerRepository.GetAsync(x => x.User.Id == model.UserId);
        var review = await _reviewRepository.GetReviewAsync(x => x.Id == model.ReviewId);
        if (customer != null || review != null)
        {
            var like = await _likeRepository.GetLikeAsync(x => x.ReviewId == model.ReviewId && x.User.Customer.UserId == model.UserId);
            if (like == null)
            {
                var newlike = new Like
                {
                    UserId = customer.User.Id,
                    ReviewId = review.Id

                };
                await _likeRepository.CreateAsync(newlike);
                await _likeRepository.SaveAsync();

                return new BaseResponse<LikeDto>
                {
                    Message = "",
                    Status = true
                };
            }
            var result = await UpdateLike(model.ReviewId, model.UserId);
            if (result.Status == true)
            {
                return new BaseResponse<LikeDto>
                {
                    Message = "Successful",
                    Status = true
                };
            }
        }
        return new BaseResponse<LikeDto>
        {
            Message = "Failed",
            Status = false
        };
    }

    public async Task<BaseResponse<LikeDto>> GetLikesByReviewIdAsync(string reveiwId)
    {
        var get = await _likeRepository.GetLikeAsync(x => x.Review.Id == reveiwId);
        if (get != null)
        {
            var likes = await _likeRepository.GetLikesAsync(get.Id);
            return new BaseResponse<LikeDto>
            {
                Message = "Successful",
                Status = true,
                Data = new LikeDto
                {
                    NumberOfLikes = likes.Count(),
                    CustomerDto = likes.Select(x => new CustomerDto
                    {
                        Users = new UserDto
                        {
                            Id = x.User.Id,
                        }
                    }).ToList()
                }
            };

        }
        return new BaseResponse<LikeDto>
        {
            Message = "Failed",
            Status = false,
        };
    }

    public async Task<BaseResponse<LikeDto>> UpdateLike(string reveiwId, string customerId)
    {
        var like = await _likeRepository.GetLikeAsync(x => x.Review.Id == reveiwId && x.User.Customer.UserId == customerId);
        if (like != null)
        {
            if (like.IsDeleted == true)
            {
                like.IsDeleted = false;
            }
            else if (like.IsDeleted == false)
            {
                like.IsDeleted = true;
            }
            await _likeRepository.UpdateAsync(like);
            await _likeRepository.SaveAsync();
            return new BaseResponse<LikeDto>
            {
                Message = "Successful",
                Status = true
            };
        }
        return new BaseResponse<LikeDto>
        {
            Message = "Failed",
            Status = false

        };

    }
}
