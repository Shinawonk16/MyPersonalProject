using Application.Dto;

namespace Application.Abstractions.IService;

public interface IReviewService
{

     Task<BaseResponse<ReviewDto>> AddReviewAsync(CreateReviewRequestModel model,string customerId);
    Task<BaseResponse<ICollection<ReviewDto>>> GetAllReviewAsync();
    Task<BaseResponse<ReviewDto>> DeleteAsync( string id);

    Task<BaseResponse<ICollection<ReviewDto>>> GetAllUnSeenReviewAsync();
    Task<BaseResponse<ReviewDto>> GetAllReviewByIdAsync(string id);
    Task<BaseResponse<ICollection<ReviewDto>>> GetAllReviewByCustomerIdAsync(string customerId);
    Task<BaseResponse<ReviewDto>> UpdateReviewAsync(string id,UpdateReviewRequestModel model);

}
