using Application.Dto;

namespace Application.Abstractions.IService;

public interface IManagerService
{
    Task<BaseResponse<ManagerDto>> CreateManagersAsync(CreateManagerRequestModel model);
    Task<BaseResponse<ManagerDto>> CompleteRegistrationAsync(CompleteManagerRegistrationRequestModel model);
    Task<BaseResponse<ManagerDto>> UpdateManagerAsync(string id, UpdateManagerRequestModel model);
    Task<BaseResponse<ManagerDto>> GetByIdAsync(string id);
    Task<BaseResponse<ManagerDto>> DeleteAsync(string id);

    Task<BaseResponse<ManagerDto>> GetByRoleAsync(string roleId);
    Task<BaseResponse<IEnumerable<ManagerDto>>> GetSelectedAsync();
}
