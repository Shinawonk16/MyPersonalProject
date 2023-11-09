using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Dto;
using Domain.Entities;

namespace Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerRepository _productRepository;

    public ReviewService(IReviewRepository reviewRepository, ICustomerRepository customerRepository, ICustomerRepository productRepository)
    {
        _reviewRepository = reviewRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
    }


    public async Task<BaseResponse<ReviewDto>> AddReviewAsync(CreateReviewRequestModel model, string customerId)
    {
        var get = await _customerRepository.GetAsync(customerId);
        if (get != null)
        {
            var review = new Review
            {
                Comment = model.Comment,
                CustomerId = get.User.Customer.Id,
                Seen = false,
                CreatedAt = DateTime.UtcNow,
            };
            await _reviewRepository.CreateAsync(review);
            await _reviewRepository.SaveAsync();
            return new BaseResponse<ReviewDto>
            {
                Message = "your review was Submitted successfully",
                Status = true
            };
        }
        return new BaseResponse<ReviewDto>
        {
            Message = "Try again",
            Status = false
        };
    }

    public async Task<BaseResponse<ReviewDto>> DeleteAsync(string id)
    {
        var get = await _reviewRepository.GetReviewAsync(x => x.Id == id);
        if (get != null)
        {
            get.IsDeleted = true;
            get.DeletedBy = "Customer";
            await _reviewRepository.SaveAsync();
            return new BaseResponse<ReviewDto>
            {
                Message = "Successfully deleted",
                Status = true,
            };
        }
        return new BaseResponse<ReviewDto> { Message = "Fail to deleted", Status = false, };

    }

    public async Task<BaseResponse<ICollection<ReviewDto>>> GetAllReviewAsync()
    {
        var getAll = await _reviewRepository.GetAllReviewAsync();
        if (getAll != null)
        {
            foreach (var item in getAll)
            {
                if ((DateTime.Now - item.CreatedAt).TotalSeconds < 60)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedAt).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - item.CreatedAt).TotalSeconds > 60 && (DateTime.Now - item.CreatedAt).TotalHours < 1)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedAt).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - item.CreatedAt).TotalMinutes > 60 && (DateTime.Now - item.CreatedAt).TotalDays < 1)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedAt).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - item.CreatedAt).TotalHours > 24 && (DateTime.Now - item.CreatedAt).TotalDays < 30)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedAt).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - item.CreatedAt).TotalDays > 30 && (DateTime.Now - item.CreatedAt).TotalDays <= 365)
                {
                    item.PostedTime = ((int)(DateTime.Now - item.CreatedAt).TotalDays / 30) + " " + "Months ago";
                }

            }
            return new BaseResponse<ICollection<ReviewDto>>
            {
                Message = "Reviews found",
                Status = true,
                Data = getAll.Select(x => new ReviewDto
                {
                    Id = x.Id,
                    Comment = x.Comment,
                    CreatedAt = x.CreatedAt,
                    CustomerDto = new CustomerDto
                    {
                        Users = new UserDto
                        {
                            UserName = $"{x.Customer.User.FirstName} {x.Customer.User.LastName}",
                            ProfilePicture = x.Customer.User.ProfilePicture,

                        }
                    },

                }).ToList()
            };
        }
        return new BaseResponse<ICollection<ReviewDto>> { Message = "Failed", Status = true };

    }

    public async Task<BaseResponse<ICollection<ReviewDto>>> GetAllReviewByCustomerIdAsync(string customerId)
    {
        var customer = await _customerRepository.GetAsync(customerId);
        if (customer == null)
        {
            return new BaseResponse<ICollection<ReviewDto>>
            {
                Message = "Customer not found",
                Status = false,
            };
        }
        var reviews = await _reviewRepository.GetAllReviewByCustomerIdAsync(customer.Id);
        if (reviews.Count() == 0)
        {
            return new BaseResponse<ICollection<ReviewDto>>
            {
                Message = "Review not found",
                Status = false
            };
        }
        return new BaseResponse<ICollection<ReviewDto>>
        {
            Message = "Reviews found",
            Status = true,
            Data = reviews.Select(x => new ReviewDto
            {
                Id = x.Id,
                Comment = x.Comment,
                CreatedAt = x.CreatedAt,
                CustomerDto = new CustomerDto
                {
                    Users = new UserDto
                    {
                        UserName = $"{x.Customer.User.FirstName} {x.Customer.User.LastName}",
                        ProfilePicture = x.Customer.User.ProfilePicture,

                    }
                },
            }).ToList()
        };

    }

    public async Task<BaseResponse<ReviewDto>> GetAllReviewByIdAsync(string id)
    {
        var review = await _reviewRepository.GetReviewAsync(x => x.Id == id);
        if (review == null)
        {
            return new BaseResponse<ReviewDto>
            {
                Message = "Review not found",
                Status = false
            };
        }
        return new BaseResponse<ReviewDto>
        {
            Message = "Review found successfully",
            Status = true,
            Data = new ReviewDto
            {
                Id = review.Id,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                CustomerDto = new CustomerDto
                {
                    Users = new UserDto
                    {
                        UserName = $"{review.Customer.User.FirstName} {review.Customer.User.LastName}",
                        ProfilePicture = review.Customer.User.ProfilePicture,

                    }
                },
            }
        };
    }

    public async Task<BaseResponse<ICollection<ReviewDto>>> GetAllUnSeenReviewAsync()
    {
        double s = 2;
        int x = (int)s;
        var reviews = await _reviewRepository.GetAllSelectedReviewAsync(x => x.Seen == false);
        if (reviews.Count == 0)
        {
            return new BaseResponse<ICollection<ReviewDto>>
            {
                Message = "no unseen Reviews yet",
                Status = false
            };
        }
        foreach (var item in reviews)
        {
            if ((DateTime.Now - item.CreatedAt).TotalSeconds < 60)
            {
                item.PostedTime = (int)(DateTime.Now - item.CreatedAt).TotalSeconds + " " + "Sec ago";
            }
            if ((DateTime.Now - item.CreatedAt).TotalSeconds > 60 && (DateTime.Now - item.CreatedAt).TotalHours < 1)
            {
                item.PostedTime = (int)(DateTime.Now - item.CreatedAt).TotalMinutes + " " + "Mins ago";
            }
            if ((DateTime.Now - item.CreatedAt).TotalMinutes > 60 && (DateTime.Now - item.CreatedAt).TotalDays < 1)
            {
                item.PostedTime = (int)(DateTime.Now - item.CreatedAt).TotalHours + " " + "Hours ago";
            }
            if ((DateTime.Now - item.CreatedAt).TotalHours > 24 && (DateTime.Now - item.CreatedAt).TotalDays < 30)
            {
                item.PostedTime = (int)(DateTime.Now - item.CreatedAt).TotalDays + " " + "Days ago";
            }
            if ((DateTime.Now - item.CreatedAt).TotalDays > 30 && (DateTime.Now - item.CreatedAt).TotalDays <= 365)
            {
                item.PostedTime = ((int)(DateTime.Now - item.CreatedAt).TotalDays / 30) + " " + "Months ago";
            }

        }
        return new BaseResponse<ICollection<ReviewDto>>
        {
            Message = "Reviews found",
            Status = true,
            Data = reviews.Select(x => new ReviewDto
            {
                Seen = x.Seen,
                Id = x.Id,
                Comment = x.Comment,
                CustomerDto = new CustomerDto
                {
                    Users = new UserDto
                    {
                        UserName = $"{x.Customer.User.FirstName} {x.Customer.User.LastName}",
                        ProfilePicture = x.Customer.User.ProfilePicture,

                    }
                },

            }).ToList()
        };

    }

    public async Task<BaseResponse<ReviewDto>> UpdateReviewAsync(string id, UpdateReviewRequestModel model)
    {
        var review = await _reviewRepository.GetReviewAsync(x => x.Id == id);
        if (review == null)
        {
            return new BaseResponse<ReviewDto>
            {
                Message = "review not found",
                Status = false
            };
        }
        review.Seen = true;
        await _reviewRepository.UpdateAsync(review);
        await _reviewRepository.SaveAsync();
        return new BaseResponse<ReviewDto>
        {
            Message = "Review updated successfully",
            Status = true
        };


    }

}
