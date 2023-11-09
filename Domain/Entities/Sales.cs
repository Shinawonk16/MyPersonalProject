using Domain.Common;

namespace Domain.Entities;

public class Sales : BaseEntity
{
    public decimal AmountPaid { get; set; }
    public string OrderId { get; set; }
    public Order Order { get; set; }
}
