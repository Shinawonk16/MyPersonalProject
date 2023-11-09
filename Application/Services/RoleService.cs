using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Dto;
using Domain.Entities;

namespace Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;

    public RoleService(IRoleRepository roleRepository, IUserRepository userRepository)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
    }

    public async Task<BaseResponse<RoleDto>> AddRoleAsync(CreateRoleRequestModel model)
    {
        var get = await _roleRepository.GetRoleAsync(x => x.RoleName == model.RoleName.ToLower());
        if (get == null)
        {
            var role = new Role
            {
                RoleName = model.RoleName,
                RoleDescription = model.RoleDescription,
            };
            await _roleRepository.CreateAsync(role);
            await _roleRepository.SaveAsync();

            return new BaseResponse<RoleDto>
            {
                Message = "Added Successfully",
                Status = true,
                Data = new RoleDto
                {
                    RoleName = model.RoleName,
                    RoleDescription = model.RoleDescription,
                }
            };

        }
        return new BaseResponse<RoleDto>
        {
            Message = "Already exist",
            Status = false
        };
    }

    public async Task<BaseResponse<RoleDto>> DeleteAsync(string id)
    {
        var get = await _roleRepository.GetRoleAsync(id);
        if (get != null)
        {
            get.IsDeleted = true;
            get.DeletedBy = "Super-Admin";
            await _roleRepository.SaveAsync();
            return new BaseResponse<RoleDto>
            {
                Message = "Successfully deleted",
                Status = true,
            };
        }
        return new BaseResponse<RoleDto> { Message = "Fail to deleted", Status = false, };
    }

    public async Task<BaseResponse<IList<RoleDto>>> GetAllRole()
    {
        var get = await _roleRepository.GetAllRoleAsync();
        if (get != null)
        {
            return new BaseResponse<IList<RoleDto>>
            {
                Message = "Found Successfully",
                Status = true,
                Data = get.Select(x =>
                new RoleDto
                {
                    RoleDescription = x.RoleDescription,
                    RoleName = x.RoleName,
                }).ToList()
            };
        }
        return new BaseResponse<IList<RoleDto>>
        {
            Message = "",
            Status = false,
        };
    }

    public async Task<BaseResponse<RoleDto>> GetRoleByUserIdAsync(string userId)
    {
        var get = await _roleRepository.GetRoleAsync(userId);
        if (get != null)
        {
            return new BaseResponse<RoleDto>
            {
                Message = "Found Successfully",
                Status = true,
                Data = new RoleDto
                {
                    RoleDescription = get.RoleDescription,
                    RoleName = get.RoleName,
                }
            };
        }
        return new BaseResponse<RoleDto>
        {
            Message = "",
            Status = false,
        };
    }

    public async Task<BaseResponse<RoleDto>> UpdateRoleAsync(UpdateRoleRequestModel model, string id)
    {
        var get = await _roleRepository.GetRoleAsync(x => x.Id == id);
        if (get != null)
        {
            return new BaseResponse<RoleDto>
            {
                Message = "Update Successfully",
                Status = true,
                Data = new RoleDto
                {
                    RoleName = get.RoleName,
                    RoleDescription = get.RoleDescription,
                }
            };
        }
        return new BaseResponse<RoleDto>
        {
            Message = "operation failed",
            Status = false,

        };
    }
}

