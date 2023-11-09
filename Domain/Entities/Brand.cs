using Domain.Common;

namespace Domain.Entities;

public class Brand : BaseEntity
{
    public string BrandName { get; set; }
    public string BrandDescription { get; set; }
    public ICollection<Product> Products { get; set; }
}
