using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;
public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<bool> CheckIfEmailExistAsync(string email);
    Task<Customer> GetAsync(string id);
    Task<IEnumerable<Customer>> GetAllCustomerAsync();
    Task<Customer> GetAsync(Expression<Func<Customer, bool>> expression);
    Task<IEnumerable<Customer>> GetSelectedAsync(Expression<Func<Customer, bool>> expression);
}
