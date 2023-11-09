using Application.Dto;

namespace Application.Abstractions.IService;

public interface IUserService
{
    Task<BaseResponse<UserDto>> LoginUserAsync(LoginUserRequsetModel model);
    Task<BaseResponse<IList<UserDto>>> GetUsersByRoleAsync(string role);
    Task<BaseResponse<UserDto>> GetUserByTokenAsync(string token);
}
