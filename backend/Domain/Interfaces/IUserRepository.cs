using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces;
public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUserNameAsync(string userName);

    Task<int> CreateAsync(User user);
    Task UpdateAsync(User user);

    Task<int?> GetRoleIdByNameAsync(string roleName);

    Task<IEnumerable<int>> GetEmployeeIdsByUserIdAsync(int userId);
    Task AddUserEmployeeAsync(int userId, int employeeId);
    Task RemoveUserEmployeeAsync(int userId, int employeeId);
}