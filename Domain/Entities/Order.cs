using Domain.Common;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public string CustomerId { get; set; }
    public bool IsDelivered { get; set; }
    public Customer Customer { get; set; }
    public Payment Payment { get; set; }
    public Sales Sales { get; set; }
    public Address Address { get; set; }
    public string AddressId { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();
}
