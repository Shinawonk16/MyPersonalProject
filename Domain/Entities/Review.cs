using Domain.Common;

namespace Domain.Entities;

public class Review : BaseEntity
{
    public bool Seen { get; set; } = false;
    public string Comment { get; set; }
    public Customer Customer { get; set; }
    public string CustomerId { get; set; }
    public Product Product { get; set; }
    public string PostedTime { get; set; }
    public ICollection<Like> Likes { get; set; } = new HashSet<Like>();
}
