using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;

public interface IManagerRepository : IBaseRepository<Manager>
{
    Task<UserRole> GetManagerAsync(string id);
    Task<UserRole> GetManagerByRoleAsync(string roleId);

    Task<bool> CheckIfEmailExistAsync(string email);
    Task<IEnumerable<UserRole>> GetAllManagerAsync();
    Task<Manager> GetAsync(Expression<Func<Manager, bool>> expression);
    Task<IEnumerable<Manager>> GetSelectedAsync(Expression<Func<Manager, bool>> expression);

}