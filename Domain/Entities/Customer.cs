using Domain.Common;

namespace Domain.Entities;

public class Customer:BaseEntity
{
    public User User { get; set; }
    public string UserId { get; set; }
    public List<Review> Review { get; set; }
    public List<Verification> Verifications { get; set; }
    public List<Payment> Payment { get; set; }
    public List<Order> Order { get; set; }
    public DateTime LastPurchaseDate { get; set; }
    public int TotalVisits { get; set; }
}
