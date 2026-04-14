using System.Data;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public EmployeeRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT Id, CompanyId, FirstName, LastName, Email
            FROM Employees
            WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Employee>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Employee>> GetByCompanyIdAsync(int companyId)
    {
        const string sql = @"
            SELECT Id, CompanyId, FirstName, LastName, Email
            FROM Employees
            WHERE CompanyId = @CompanyId;";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Employee>(sql, new { CompanyId = companyId });
    }

    public async Task<IEnumerable<Employee>> SearchByNameAsync(string? firstName, string? lastName)
    {
        const string sql = @"
            SELECT Id, CompanyId, FirstName, LastName, Email
            FROM Employees
            WHERE (@FirstName IS NULL OR FirstName LIKE '%' + @FirstName + '%')
              AND (@LastName IS NULL OR LastName LIKE '%' + @LastName + '%');";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Employee>(sql, new { FirstName = firstName, LastName = lastName });
    }

    public async Task<int> CreateAsync(Employee employee)
    {
        const string sql = @"
            INSERT INTO Employees (CompanyId, FirstName, LastName, Email)
            VALUES (@CompanyId, @FirstName, @LastName, @Email);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        using var connection = _connectionFactory.CreateConnection();
        var id = await connection.ExecuteScalarAsync<int>(sql, employee);
        return id;
    }

    public async Task UpdateAsync(Employee employee)
    {
        const string sql = @"
            UPDATE Employees
            SET CompanyId = @CompanyId,
                FirstName = @FirstName,
                LastName = @LastName,
                Email = @Email
            WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, employee);
    }

    public async Task DeleteAsync(int id)
    {
        const string sql = @"DELETE FROM Employees WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}