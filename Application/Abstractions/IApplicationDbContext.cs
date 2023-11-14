using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<User> User { get; set; }
    DbSet<Customer> Customers { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    DbSet<ProductRequest> ProductRequests { get; set; }

    DbSet<Role> Roles { get; set; }
    public DbSet<Request> Requests { get; set; }
    DbSet<OrderProduct> OrderProducts { get; set; }
    DbSet<Order> Orders { get; set; }
    DbSet<Manager> Managers { get; set; }
    DbSet<Like> Likes { get; set; }
    DbSet<Address> Addresses { get; set; }
    DbSet<Brand> Brands { get; set; }
    DbSet<Payment> Payments { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<Review> Reviews { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Sales> Sales { get; set; }
    DbSet<Verification> Verifications { get; set; }

}
