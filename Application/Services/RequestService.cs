using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Dto;
using Domain.Entities;
using Domain.Enum;

namespace Application.Services;

public class RequestService : IRequestService
{
    private readonly IRequestRepository _requestRepository;
    private readonly IProductRepository _productRepository;
    private readonly IManagerRepository _managerReopsitory;

    public RequestService(IRequestRepository requestRepository, IProductRepository productRepository, IManagerRepository managerReopsitory)
    {
        _requestRepository = requestRepository;
        _productRepository = productRepository;
        _managerReopsitory = managerReopsitory;
    }

    public async Task<BaseResponse<RequestDto>> ApproveRequestAsync(string id)
    {
        var request = await _requestRepository.GetRequestAsync(id);
        if (request == null)
        {
            return new BaseResponse<RequestDto>
            {
                Message = "Request is Invalid",
                Status = false,
            };
        }
        request.ApprovalStatus = ApprovalStatus.Approved;
        request.IsApproved = true;
        await _requestRepository.SaveAsync();
        return new BaseResponse<RequestDto>
        {
            Message = "Approved Successfully",
            Status = true,
            Data = new RequestDto
            {
                IsApproved = true
            }
        };


    }

    public async Task<BaseResponse<RequestDto>> CalculateRequestCostForTheMonth()
    {
        var request = await _requestRepository.GetSelectedRequestAsync(x => x.IsApproved == true && x.ApprovalStatus == ApprovalStatus.Approved);
        if (request == null && request.Cost == 0)
        {
            return new BaseResponse<RequestDto>
            {
                Message = "Not found ",
                Status = false,
            };
        }
        return new BaseResponse<RequestDto>
        {
            Message = "found Successfully",
            Status = true,
            Data = new RequestDto
            {
                Cost = request.Cost,
            }
        };
    }

    public async Task<BaseResponse<RequestDto>> CalculateRequestCostForThYear()
    {
        var request = await _requestRepository.GetSelectedRequestAsync(x => x.IsApproved == true && x.ApprovalStatus == ApprovalStatus.Approved);
        if (request == null && request.Cost == 0)
        {
            return new BaseResponse<RequestDto>
            {
                Message = "Not found ",
                Status = false,
            };
        }
        return new BaseResponse<RequestDto>
        {
            Message = "found Successfully",
            Status = true,
            Data = new RequestDto
            {
                Cost = request.Cost,
            }
        };
    }


    public async Task<BaseResponse<RequestDto>> CreateRequestAsync(string managerId, CreateRequestRequestModel model)
    {
        var request = new Request
        {
            Cost = model.Cost,
            Quantity = model.Quantity,
            AdditionalNote = model.AdditionalNote,

        };
        var get = await _productRepository.GetAsync(x => x.ProductName.ToLower() == model.ProductName.ToLower());
        if (get == null)
        {
            return new BaseResponse<RequestDto>
            {
                Message = "Failed",
                Status = false,
            };
        }
        await _requestRepository.CreateAsync(request);
        await _requestRepository.SaveAsync();
        return new BaseResponse<RequestDto>
        {
            Message = "Successful",
            Status = true,
            Data = new RequestDto
            {
                AdditionalNote = request.AdditionalNote,
                Cost = request.Cost,
                Quantity = request.Quantity,
                CreatedAt = request.CreatedAt
            }
        };
    }




    public async Task<BaseResponse<RequestDto>> DeleteAsync(string id)
    {
        var get = await _requestRepository.GetRequestAsync(id);
        if (get != null)
        {
            get.IsDeleted = true;
            await _requestRepository.SaveAsync();
            return new BaseResponse<RequestDto>
            {
                Message = "Successfully deleted",
                Status = true,
            };
        }
        return new BaseResponse<RequestDto> { Message = "Fail to deleted", Status = false, }; throw new NotImplementedException();

    }

    public async Task<BaseResponse<IEnumerable<RequestDto>>> GetAllAprovedRequestForTheMonthAsync(int month, int year)
    {
        var request = await _requestRepository.GetAllSelected(x => x.IsApproved == true && x.ApprovalStatus == ApprovalStatus.Approved && x.CreatedAt.Year == year && x.CreatedAt.Month == month);
        if (request != null)
        {
            return new BaseResponse<IEnumerable<RequestDto>>
            {
                Message = "Retrived Successfully",
                Status = true,
                Data = request.Select(x => new RequestDto
                {
                    Id = x.Id,
                    Cost = x.Cost,
                    Quantity = x.Quantity,
                    // QuantiityRemaining = x.QuantiityRemaining,
                    // Name = x.Name,
                    AdditionalNote = x.AdditionalNote,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedAt.ToLongDateString(),
                    AdminImage = x.Manager.User.ProfilePicture,
                    AdminName = x.Manager.User.UserName,

                }).ToList()

            };
        }
        return new BaseResponse<IEnumerable<RequestDto>>
        {
            Message = "Invalid Request",
            Status = false,
        };
    }

    public async Task<BaseResponse<IEnumerable<RequestDto>>> GetAllAprovedRequestForTheYear(int year)
    {
        var request = await _requestRepository.GetAllSelected(x => x.IsApproved == true && x.ApprovalStatus == ApprovalStatus.Approved && x.CreatedAt.Year == year);
        if (request != null)
        {
            return new BaseResponse<IEnumerable<RequestDto>>
            {
                Message = "Retrived Successfully",
                Status = true,
                Data = request.Select(x => new RequestDto
                {
                    Id = x.Id,
                    Cost = x.Cost,
                    Quantity = x.Quantity,
                    // QuantiityRemaining = x.QuantiityRemaining,
                    // Name = x.Name,
                    AdditionalNote = x.AdditionalNote,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedAt.ToLongDateString(),
                    AdminImage = x.Manager.User.ProfilePicture,
                    AdminName = x.Manager.User.UserName,

                }).ToList()

            };
        }
        return new BaseResponse<IEnumerable<RequestDto>>
        {
            Message = "Invalid Request",
            Status = false,
        };

    }


    public async Task<BaseResponse<IEnumerable<RequestDto>>> GetAllAsync()
    {
        var get = await _requestRepository.GetAllRequestAsync(); if (get != null)
        {
            return new BaseResponse<IEnumerable<RequestDto>>
            {
                Message = "Successful",
                Status = true,
                Data = get.Select(x => new RequestDto
                {
                    Cost = x.Cost,
                    AdditionalNote = x.AdditionalNote,
                    Quantity = x.Quantity,
                    CreatedAt = x.CreatedAt,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedAt.ToLongDateString(),
                    AdminImage = x.Manager.User.ProfilePicture,
                    AdminName = x.Manager.User.UserName,
                }).ToList()
            };
        }
        return new BaseResponse<IEnumerable<RequestDto>> { Message = " Failed", Status = false };

    }

    public async Task<BaseResponse<IEnumerable<RequestDto>>> GetAllPendingRequest()
    {
        var request = await _requestRepository.GetAllSelected(x => x.ApprovalStatus == ApprovalStatus.Pending && x.IsApproved == false);
        if (request != null)
        {
            return new BaseResponse<IEnumerable<RequestDto>>
            {
                Message = "Retrived Successful",
                Status = true,
                Data = request.Select(x => new RequestDto
                {
                    Cost = x.Cost,
                    AdditionalNote = x.AdditionalNote,
                    Quantity = x.Quantity,
                    CreatedAt = x.CreatedAt,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedAt.ToLongDateString(),
                    AdminImage = x.Manager.User.ProfilePicture,
                    AdminName = x.Manager.User.UserName,
                }).ToList()
            };
        }
        return new BaseResponse<IEnumerable<RequestDto>> { Message = " Failed", Status = false };

    }

    public Task<BaseResponse<IEnumerable<RequestDto>>> GetAllRejectedReqestForTheYearAsync(int year)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<IEnumerable<RequestDto>>> GetAllRejectedRequestForTheMonthAsync(int month)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<RequestDto>> GetAsync()
    {
        throw new NotImplementedException();
    }


    public async Task<BaseResponse<RequestDto>> GetByIdAsync(string id)
    {
        var x = await _requestRepository.GetRequestAsync(id); if (x != null)
        {
            return new BaseResponse<RequestDto>
            {
                Message = "Successful",
                Status = true,
                Data = new RequestDto
                {
                    Cost = x.Cost,
                    AdditionalNote = x.AdditionalNote,
                    Quantity = x.Quantity,
                    CreatedAt = x.CreatedAt,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedAt.ToLongDateString(),
                    AdminImage = x.Manager.User.ProfilePicture,
                    AdminName = x.Manager.User.UserName,
                }
            };
        }
        return new BaseResponse<RequestDto> { Message = " Failed", Status = false };

    }

    public async Task<BaseResponse<IEnumerable<RequestDto>>> GetSelectedAsync()
    {
        var approved = await _requestRepository.GetAllSelected(x => x.IsApproved == true && x.ApprovalStatus == ApprovalStatus.Approved); if (approved != null)
        {
            return new BaseResponse<IEnumerable<RequestDto>>
            {
                Message = "Successful",
                Status = true,
                Data = approved.Select(x => new RequestDto
                {
                    Cost = x.Cost,
                    AdditionalNote = x.AdditionalNote,
                    Quantity = x.Quantity,
                    CreatedAt = x.CreatedAt,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedAt.ToLongDateString(),
                    AdminImage = x.Manager.User.ProfilePicture,
                    AdminName = x.Manager.User.UserName,
                }).ToList()
            };
        }
        return new BaseResponse<IEnumerable<RequestDto>> { Message = " Failed", Status = false };

    }

    public async Task<BaseResponse<RequestDto>> RejectRequestAsync(string id, RejectRequestRequestModel model)
    {
        var request = await _requestRepository.GetRequestAsync(id);
        if (request != null)
        {
            request.ApprovalStatus = ApprovalStatus.Rejected;
            request.IsApproved = true;
            await _requestRepository.SaveAsync();
            return new BaseResponse<RequestDto>
            {
                Message = "Rejected Successfully",
                Status = true,
                Data = new RequestDto
                {
                    EnumApprovalStatus = ApprovalStatus.Rejected,
                    StringApprovalStatus = request.ApprovalStatus.ToString()
                }
            };
        }
        return new BaseResponse<RequestDto>
        {
            Message = "Not found",
            Status = false,
        };
    }


    public async Task<BaseResponse<RequestDto>> UpdateRequestAsync(string id, UpdateRequestRequestModel model)
    {
        var update = await _requestRepository.GetRequestAsync(id);
        if (update != null)
        {
            var request = new Request
            {
                Cost = model.Cost,
                Quantity = model.Quantity,
                AdditionalNote = model.AdditionalNote,
                // CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,


            };
            var get = await _productRepository.GetAsync(x => x.ProductName.ToLower() == model.ProductName.ToLower());
            if (get == null)
            {
                return new BaseResponse<RequestDto>
                {
                    Message = "Failed",
                    Status = false,
                };
            }
            await _requestRepository.UpdateAsync(request);
            await _requestRepository.SaveAsync();
            return new BaseResponse<RequestDto>
            {
                Message = "Successful",
                Status = true,
                Data = new RequestDto
                {
                    AdditionalNote = request.AdditionalNote,
                    Cost = request.Cost,
                    Quantity = request.Quantity,
                    CreatedAt = request.CreatedAt,
                    UpdatedAt = request.UpdatedAt,
                    Product = new ProductDto
                    {
                        // ProductName = request.Product.ProductName
                    }

                }
            };
        }
        return new BaseResponse<RequestDto>
        {
            Message = "Failed",
            Status = false,

        };
    }
}

