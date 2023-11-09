using Domain.Common;

namespace Domain.Entities;

public class Verification : BaseEntity
{
    public int Code { get; set; }
    public string CustomerId { get; set; }
    public Customer Customer { get; set; }
}
