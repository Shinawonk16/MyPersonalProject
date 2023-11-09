using Domain.Common;

namespace Domain.Entities;

public class Request : BaseEntity
{
    public string ProductId { get; set; }
    public string AdditionalNote { get; set; }
    public decimal Cost { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public bool IsApproved { get; set; }
}
