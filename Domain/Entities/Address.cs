using Domain.Common;

namespace Domain.Entities;

public class Address : BaseEntity
{
    public string Street { get; set; }
    public int? PostalCode { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public User User { get; set; }
}
