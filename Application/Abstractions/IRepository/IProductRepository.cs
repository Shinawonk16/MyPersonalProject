using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product> GetAsync(string id);
    Task<Product> GetAsync(Expression<Func<Product, bool>> expression);
    Task<IEnumerable<Product>> GetSelectedAsync(Expression<Func<Product, bool>> expression);
    Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryId);
    Task<IEnumerable<Product>> GetAllProductAsync();
    Task<IEnumerable<Product>> GetByPriceAsync(decimal price);
    Task<Product> GetProductByBrandAsync(string brandId);
}
