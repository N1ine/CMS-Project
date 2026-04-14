using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public UserRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT u.Id, u.UserName, u.PasswordHash, u.RoleId, r.RoleName AS Role
            FROM Users u
            LEFT JOIN Roles r ON u.RoleId = r.Id
            WHERE u.Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        const string sql = @"
            SELECT u.Id, u.UserName, u.PasswordHash, u.RoleId, r.RoleName AS Role
            FROM Users u
            LEFT JOIN Roles r ON u.RoleId = r.Id
            WHERE u.UserName = @UserName;";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { UserName = userName });
    }

    public async Task<int?> GetRoleIdByNameAsync(string roleName)
    {
        const string sql = @"SELECT Id FROM Roles WHERE RoleName = @RoleName;";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<int?>(sql, new { RoleName = roleName });
    }

    public async Task<int> CreateAsync(User user)
    {
        const string sql = @"
            INSERT INTO Users (UserName, PasswordHash, RoleId)
            VALUES (@UserName, @PasswordHash, @RoleId);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            user.UserName,
            user.PasswordHash,
            RoleId = user.RoleId
        });
    }

    public async Task UpdateAsync(User user)
    {
        const string sql = @"
            UPDATE Users
            SET UserName = @UserName,
                PasswordHash = @PasswordHash,
                RoleId = @RoleId
            WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            user.UserName,
            user.PasswordHash,
            RoleId = user.RoleId,
            user.Id
        });
    }

    public async Task<IEnumerable<int>> GetEmployeeIdsByUserIdAsync(int userId)
    {
        const string sql = @"
            SELECT EmployeeId
            FROM UserEmployees
            WHERE UserId = @UserId;";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<int>(sql, new { UserId = userId });
    }

    public async Task AddUserEmployeeAsync(int userId, int employeeId)
    {
        const string sql = @"
            IF NOT EXISTS (SELECT 1 FROM UserEmployees WHERE UserId = @UserId AND EmployeeId = @EmployeeId)
                INSERT INTO UserEmployees (UserId, EmployeeId) VALUES (@UserId, @EmployeeId);";

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, new { UserId = userId, EmployeeId = employeeId });
    }

    public async Task RemoveUserEmployeeAsync(int userId, int employeeId)
    {
        const string sql = @"
            DELETE FROM UserEmployees
            WHERE UserId = @UserId AND EmployeeId = @EmployeeId;";

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, new { UserId = userId, EmployeeId = employeeId });
    }
}