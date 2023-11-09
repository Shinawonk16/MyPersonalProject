using Application.Dto;

namespace Application.Abstractions.IService;

public interface IRoleService
{
    Task<BaseResponse<RoleDto>> AddRoleAsync(CreateRoleRequestModel model);
    Task<BaseResponse<RoleDto>> UpdateRoleAsync(UpdateRoleRequestModel model, string id);
    Task<BaseResponse<RoleDto>> GetRoleByUserIdAsync(string userId);
    Task<BaseResponse<IList<RoleDto>>> GetAllRole();
    Task<BaseResponse<RoleDto>> DeleteAsync(string id);
}
