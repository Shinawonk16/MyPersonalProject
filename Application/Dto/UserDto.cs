using Domain.Enum;

namespace Application.Dto;

public class UserDto
{
    public string FirstName { get; set; }
    public string UserName { get; set; }
    public string Id { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string ProfilePicture { get; set; }
    public Gender Gender { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }
    public string RoleDescription { get; set; }
}
public class LoginUserRequsetModel
{

    public string Password { get; set; }
    public string Email { get; set; }
}