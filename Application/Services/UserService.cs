using Application.Abstractions;
using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Auths;
using Application.Dto;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class UserService : IUserService
{

    private readonly IUserRepository _userRepository;
    private readonly IShopezyUpload _uploadImages;
    private readonly IConfiguration _config;
    private readonly IJWTAuthentication _tokenService;
    private string generatedToken = null;


    public UserService(IUserRepository userRepository, IShopezyUpload uploadImages = null, IJWTAuthentication tokenService = null, IConfiguration config = null)
    {
        _userRepository = userRepository;
        _uploadImages = uploadImages;
        _tokenService = tokenService;
        _config = config;
        // this.generatedToken = generatedToken;
    }
    public async Task<BaseResponse<UserDto>> GetUserByTokenAsync(string token)
    {
        var get = await _userRepository.GetAsync(x => x.User.Token == token);
        if (get == null)
        {
            return new BaseResponse<UserDto>
            {
                Message = "User not found",
                Status = true
            };
        }
        return new BaseResponse<UserDto>
        {
            Message = "User found successfully",
            Status = true,
            Data = new UserDto
            {
                Email = get.User.Email,
                Role = get.Role.RoleName,
                RoleDescription = get.Role.RoleDescription
            }
        };
    }

    public async Task<BaseResponse<IList<UserDto>>> GetUsersByRoleAsync(string role)
    {
        var get = await _userRepository.GetUserByRoleAsync(role);
        if (get != null)
        {
            return new BaseResponse<IList<UserDto>>
            {
                Message = "Found Successfully",
                Status = true,
                Data = get.Select(x => new UserDto
                {
                    UserName = $"{x.User.FirstName} {x.User.LastName}",
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    Id = x.User.Id,
                    ProfilePicture = x.User.ProfilePicture,
                    Role = x.Role.RoleName
                }
                ).ToList()

            };
        }

        return new BaseResponse<IList<UserDto>>
        {
            Message = "Not Found",
            Status = false
        };
    }

    public async Task<BaseResponse<UserDto>> LoginUserAsync(LoginUserRequsetModel model)
    {
        var login = await _userRepository.GetAsync(x => x.User.Email == model.Email);
        if (login != null)
        {
            var test = BCrypt.Net.BCrypt.Verify(model.Password, login.User.Password);
            if (test)
            {
                var user = new UserDto
                {
                    UserName = $"{login.User.FirstName}  {login.User.LastName}",
                    Id = login.User.Id,
                    FirstName = login.User.FirstName,
                    LastName = login.User.LastName,
                    PhoneNumber = login.User.PhoneNumber,
                    Email = login.User.Email,
                    Gender = login.User.Gender,
                    ProfilePicture = login.User.ProfilePicture,
                    Role = login.Role.RoleName,
                    RoleDescription = login.Role.RoleDescription
                };
                var k = _config["Jwt:Key"].ToString();
                var i = _config["Jwt:Issuer"].ToString();
                var token = _tokenService.GenerateToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), user);
                return new BaseResponse<UserDto>
                {
                    Message = "Login Successful",
                    Status = true,
                    Data = new UserDto
                    {
                        UserName = $"{login.User.FirstName}  {login.User.LastName}",
                        Id = login.User.Id,
                        Email = login.User.Email,
                        PhoneNumber = login.User.PhoneNumber,
                        Role = login.Role.RoleName,
                        Token = token,
                        ProfilePicture = user.ProfilePicture

                    }
                };
            }

        }
        return new BaseResponse<UserDto>
        {
            Message = "Incorrect Password or Email try again",
            Status = false,
        };
    }

}
