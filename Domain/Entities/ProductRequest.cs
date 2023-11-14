using Domain.Common;

namespace Domain.Entities;

public class ProductRequest : BaseEntity
{
    public Product Product { get; set; }
    public string ProductId { get; set; }
    public Request Request { get; set; }
    public string RequestId { get; set; }
}
