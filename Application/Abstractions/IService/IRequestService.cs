using Application.Dto;

namespace Application.Abstractions.IService;

public interface IRequestService
{
    public Task<BaseResponse<RequestDto>> CreateRequestAsync(string managerId, CreateRequestRequestModel model);
    public Task<BaseResponse<RequestDto>> GetByIdAsync(string id);
    public Task<BaseResponse<RequestDto>> DeleteAsync(string id);
    public Task<BaseResponse<RequestDto>> GetAsync();
    public Task<BaseResponse<RequestDto>> UpdateRequestAsync(string id, UpdateRequestRequestModel model);
    public Task<BaseResponse<IEnumerable<RequestDto>>> GetAllAsync();
    public Task<BaseResponse<IEnumerable<RequestDto>>> GetSelectedAsync();
    public Task<BaseResponse<RequestDto>> ApproveRequestAsync(string id);
    public Task<BaseResponse<IEnumerable<RequestDto>>> GetAllPendingRequest();
    public Task<BaseResponse<RequestDto>> CalculateRequestCostForThYear();
    public Task<BaseResponse<RequestDto>> CalculateRequestCostForTheMonth();
    Task<BaseResponse<IEnumerable<RequestDto>>> GetAllAprovedRequestForTheYear(int year);
    Task<BaseResponse<RequestDto>> RejectRequestAsync(string id, RejectRequestRequestModel model);
    Task<BaseResponse<IEnumerable<RequestDto>>> GetAllRejectedReqestForTheYearAsync(int year);
    Task<BaseResponse<IEnumerable<RequestDto>>> GetAllRejectedRequestForTheMonthAsync(int month);
    Task<BaseResponse<IEnumerable<RequestDto>>> GetAllAprovedRequestForTheMonthAsync(int month, int year);

}
