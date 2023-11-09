using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;
public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role> GetRoleAsync(string id);
    Task<Role> GetRoleAsync(Expression<Func<Role, bool>> expression);
    Task<IEnumerable<Role>> GetAllRoleAsync();
    Task<IEnumerable<Role>> GetSelectedRoleAsync(Expression<Func<Role, bool>> expression);

}
