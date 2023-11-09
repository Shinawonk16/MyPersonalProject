using Application.Dto;

namespace Application.Abstractions.IService;

public interface IRequestService
{
    public Task<BaseResponse<RequestDto>> CreateRequestAsync(CreateRequestRequestModel model);
    public Task<BaseResponse<RequestDto>> GetByIdAsync(string id);
    public Task<BaseResponse<RequestDto>> DeleteAsync(string id);
    public Task<BaseResponse<RequestDto>> GetAsync();
    public Task<BaseResponse<RequestDto>> UpdateRequestAsync(string id, UpdateRequestRequestModel model);
    public Task<BaseResponse<IEnumerable<RequestDto>>> GetAllAsync();
    public Task<BaseResponse<IEnumerable<RequestDto>>> GetSelectedAsync();
}
