using Domain.Common;

namespace Domain.Entities;
public class Role : BaseEntity
{
    public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    public string RoleName { get; set; }
    public string RoleDescription { get; set; }
}
