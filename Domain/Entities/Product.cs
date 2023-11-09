using Domain.Common;

namespace Domain.Entities;

public class Product:BaseEntity
{
     public Brand Brand { get; set; }
    public string BrandId { get; set; }
    public ICollection<Request> Request { get; set; } = new HashSet<Request>();
    public List<Review> Review { get; set; }
    public Category Category { get; set; }
    public string CategoryId { get; set; }
    // public Managers Managers { get; set; }
    public string ReviewId { get; set; }
    public decimal Price { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();
}
