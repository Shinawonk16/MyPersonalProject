using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {


    }

    //  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     if (!optionsBuilder.IsConfigured)
    //     {
    //         string connectionString = "server=localhost;user=root;port=3306;database=CLHFinalProjectFocus13;password=1234";
    //         optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    //     }
    // }


    public DbSet<User> Users { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<ProductRequest> ProductRequests { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Sales> Sales { get; set; }
    public DbSet<Verification> Verifications { get; set; }

}
