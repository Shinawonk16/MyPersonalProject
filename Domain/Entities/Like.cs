using Domain.Common;

namespace Domain.Entities;

public class Like : BaseEntity
{
    public string ReviewId { get; set; }
    public Review Review { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}
