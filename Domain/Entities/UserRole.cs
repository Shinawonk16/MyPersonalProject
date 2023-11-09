using Domain.Common;

namespace Domain.Entities;

public class UserRole:BaseEntity
{
    public User User { get; set; }
    public string UserId { get; set; }
    public Role Role { get; set; }
    public string RoleId { get; set; }
}
