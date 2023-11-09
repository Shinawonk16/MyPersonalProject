using Domain.Common;

namespace Domain.Entities;

public class Category : BaseEntity
{
    public string CategoryName { get; set; }
    public string CategoryDescription { get; set; }
    public List<Product> Product { get; set; }
}
