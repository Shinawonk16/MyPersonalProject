using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;

public interface IBrandRepository : IBaseRepository<Brand>
{
    Task<Brand> GetBrandAsync(string id);
    Task<Product> GetBrandByProductIdAsync(string productId);
    Task<IEnumerable<Brand>> GetBrandsAsync();
    Task<Brand> GetBrand(Expression<Func<Brand, bool>> expression);
}