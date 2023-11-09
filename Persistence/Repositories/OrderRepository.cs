using System.Linq.Expressions;
using Application.Abstractions.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context):base(context)
    {
        
    }
    public async Task<Order> GetAsync(Expression<Func<Order, bool>> expression)
    {
        return await _context.Orders
        .Include(a => a.OrderProducts)
        .SingleOrDefaultAsync(expression);
    }

    public async Task<Order> GetOrderAsync(string id)
    {
         return await _context.Orders
        .Include(a => a.OrderProducts)
        .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Order>> GetSelectedAsync(Expression<Func<Order, bool>> expression)
    {
         return await _context.Orders
        .Include(a => a.OrderProducts)
        .ToListAsync();
    }
}
