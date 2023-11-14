using Domain.Common;
using Domain.Enum;

namespace Domain.Entities;

public class Request : BaseEntity
{
    public string AdditionalNote { get; set; }
    public decimal Cost { get; set; }
    public Manager Manager { get; set; }
    // public string ManagerId { get; set; }
    public string RequestTime { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
    public bool IsApproved { get; set; }
    public ICollection<ProductRequest> ProductRequests { get; set; } = new HashSet<ProductRequest>();
}
