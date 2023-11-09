namespace Application.Dto;

public class RoleDto
{
    public IEnumerable<UserDto> UserDto { get; set; }
    public string RoleName { get; set; }
    public string RoleDescription { get; set; }

}

public class CreateRoleRequestModel
{
    public string RoleName { get; set; }
    public string RoleDescription { get; set; }
}

public class UpdateRoleRequestModel : CreateRoleRequestModel
{

}
