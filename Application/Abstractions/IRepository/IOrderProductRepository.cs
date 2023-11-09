using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;

public interface IOrderProductRepository : IBaseRepository<OrderProduct>
{
    Task<IEnumerable<OrderProduct>> GetAllOrderAsync();
    Task<IList<OrderProduct>> GetAsync(Expression<Func<OrderProduct, bool>> expression);
    Task<OrderProduct> GetOrderAsync(Expression<Func<OrderProduct, bool>> expression);

    Task<IEnumerable<OrderProduct>> GetSelectedAsync(Expression<Func<OrderProduct, bool>> expression);
    Task<IEnumerable<OrderProduct>> GetAllDeleiveredOrderByProductIdForTheYearAsync(string productId, int year);
    Task<IEnumerable<OrderProduct>> GetAllDeliveredOrderByProductIdForTheMonthAsync(string productId, int month, int year);
}
