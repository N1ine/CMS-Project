using System.Data;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CompanyRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Company?> GetByIdAsync(int id)
    {
        const string sql = @"SELECT Id, Name, Address, TaxNumber
                             FROM Companies
                             WHERE Id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Company>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        const string sql = @"SELECT Id, Name, Address, TaxNumber
                             FROM Companies";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Company>(sql);
    }

    public async Task<int> CreateAsync(Company company)
    {
        const string sql = @"
            INSERT INTO Companies (Name, Address, TaxNumber)
            VALUES (@Name, @Address, @TaxNumber);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        using var connection = _connectionFactory.CreateConnection();
        var id = await connection.ExecuteScalarAsync<int>(sql, company);
        return id;
    }

    public async Task UpdateAsync(Company company)
    {
        const string sql = @"
            UPDATE Companies
            SET Name = @Name,
                Address = @Address,
                TaxNumber = @TaxNumber
            WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, company);
    }

    public async Task DeleteAsync(int id)
    {
        const string sql = @"DELETE FROM Companies WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}