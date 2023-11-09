using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;
[Table("Admins")]
public class Manager : BaseEntity
{
    public User User { get; set; }
    public string UserId { get; set; }
    public Product Product { get; set; }
    public string ProductId { get; set; }
}
