using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Dto;
using Domain.Entities;

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


    public async Task<BaseResponse<RequestDto>> CreateRequestAsync(CreateRequestRequestModel model)
    {
        var request = new Request
        {
            Cost = model.Cost,
            Quantity = model.Quantity,
            AdditionalNote = model.AdditionalNote,
            CreatedAt = model.CreatedAt
        };
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

    public async Task<BaseResponse<IEnumerable<RequestDto>>> GetAllAsync()
    {
        var get = await _requestRepository.GetAllRequestAsync(); if (get != null)
        {
            return new BaseResponse<IEnumerable<RequestDto>> { Message = "Successful", Status = true, Data = get.Select(x => new RequestDto { Cost = x.Cost, AdditionalNote = x.AdditionalNote, Quantity = x.Quantity, CreatedAt = x.CreatedAt, UpdatedAt = x.UpdatedAt }).ToList() };
        }
        return new BaseResponse<IEnumerable<RequestDto>> { Message = " Failed", Status = false };

    }

    public Task<BaseResponse<RequestDto>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponse<RequestDto>> GetByIdAsync(string id)
    {
        var get = await _requestRepository.GetRequestAsync(id); if (get != null)
        {
            return new BaseResponse<RequestDto> { Message = "Successful", Status = true, Data = new RequestDto { Cost = get.Cost, AdditionalNote = get.AdditionalNote, Quantity = get.Quantity, CreatedAt = get.CreatedAt, UpdatedAt = get.UpdatedAt } };
        }
        return new BaseResponse<RequestDto> { Message = " Failed", Status = false };

    }

    public async Task<BaseResponse<IEnumerable<RequestDto>>> GetSelectedAsync()
    {
        var approved = await _requestRepository.GetAllSelected(x => x.IsApproved == false); if (approved != null)
        {
            return new BaseResponse<IEnumerable<RequestDto>> { Message = "Successful", Status = true, Data = approved.Select(x => new RequestDto { Cost = x.Cost, AdditionalNote = x.AdditionalNote, Quantity = x.Quantity, CreatedAt = x.CreatedAt, UpdatedAt = x.UpdatedAt }).ToList() };
        }
        return new BaseResponse<IEnumerable<RequestDto>> { Message = " Failed", Status = false };

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
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt

            };
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
                    UpdatedAt = request.UpdatedAt

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


