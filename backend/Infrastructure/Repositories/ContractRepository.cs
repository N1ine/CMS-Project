using System.Data;
using Dapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ContractRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Contract?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT Id, CompanyId, EmployeeId, Position, Description, StartDate, EndDate, Wage
            FROM Contracts
            WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Contract>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Contract>> GetAllAsync(int? employeeId = null)
    {
        const string sql = @"
            SELECT Id, CompanyId, EmployeeId, Position, Description, StartDate, EndDate, Wage
            FROM Contracts
            WHERE (@EmployeeId IS NULL OR EmployeeId = @EmployeeId);";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Contract>(sql, new { EmployeeId = employeeId });
    }

    public async Task<IEnumerable<Contract>> GetByEmployeeIdAsync(int employeeId)
    {
        const string sql = @"
            SELECT Id, CompanyId, EmployeeId, Position, Description, StartDate, EndDate, Wage
            FROM Contracts
            WHERE EmployeeId = @EmployeeId;";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Contract>(sql, new { EmployeeId = employeeId });
    }

    public async Task<IEnumerable<Contract>> SearchByPositionAsync(string position)
    {
        const string sql = @"
            SELECT Id, CompanyId, EmployeeId, Position, Description, StartDate, EndDate, Wage
            FROM Contracts
            WHERE Position LIKE '%' + @Position + '%';";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Contract>(sql, new { Position = position });
    }

    public async Task<IEnumerable<Contract>> SearchByEmployeeNameAsync(string? firstName, string? lastName)
    {
        const string sql = @"
            SELECT c.Id, c.CompanyId, c.EmployeeId, c.Position, c.Description, c.StartDate, c.EndDate, c.Wage
            FROM Contracts c
            INNER JOIN Employees e ON c.EmployeeId = e.Id
            WHERE (@FirstName IS NULL OR e.FirstName LIKE '%' + @FirstName + '%')
              AND (@LastName IS NULL OR e.LastName LIKE '%' + @LastName + '%');";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Contract>(sql, new { FirstName = firstName, LastName = lastName });
    }

    public async Task<IEnumerable<Contract>> GetByStatusAsync(ContractStatus status)
    {
        var now = DateTime.UtcNow.Date;

        const string baseSql = @"
            SELECT Id, CompanyId, EmployeeId, Position, Description, StartDate, EndDate, Wage
            FROM Contracts
            WHERE 1 = 1
";

        string condition = status switch
        {
            ContractStatus.NotStarted => " AND StartDate > @Now",
            ContractStatus.Active => " AND StartDate <= @Now AND (EndDate IS NULL OR EndDate >= @Now)",
            ContractStatus.Finished => " AND EndDate IS NOT NULL AND EndDate < @Now",
            _ => ""
        };

        var sql = baseSql + condition + ";";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Contract>(sql, new { Now = now });
    }

    public async Task<IEnumerable<Contract>> GetByDateRangeAsync(
        DateTime? startDateFrom,
        DateTime? startDateTo,
        DateTime? endDateFrom,
        DateTime? endDateTo)
    {
        var sql = @"
            SELECT Id, CompanyId, EmployeeId, Position, Description, StartDate, EndDate, Wage
            FROM Contracts
            WHERE 1 = 1
            ";

        var parameters = new DynamicParameters();

        if (startDateFrom.HasValue)
        {
            sql += " AND StartDate >= @StartDateFrom";
            parameters.Add("@StartDateFrom", startDateFrom.Value.Date);
        }

        if (startDateTo.HasValue)
        {
            sql += " AND StartDate <= @StartDateTo";
            parameters.Add("@StartDateTo", startDateTo.Value.Date);
        }

        if (endDateFrom.HasValue)
        {
            sql += " AND EndDate >= @EndDateFrom";
            parameters.Add("@EndDateFrom", endDateFrom.Value.Date);
        }

        if (endDateTo.HasValue)
        {
            sql += " AND EndDate <= @EndDateTo";
            parameters.Add("@EndDateTo", endDateTo.Value.Date);
        }

        sql += ";";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Contract>(sql, parameters);
    }

    public async Task<int> CreateAsync(Contract contract)
    {
        const string sql = @"
            INSERT INTO Contracts (CompanyId, EmployeeId, Position, Description, StartDate, EndDate, Wage, ContractStatusId)
            VALUES (@CompanyId, @EmployeeId, @Position, @Description, @StartDate, @EndDate, @Wage, @ContractStatusId);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        using var connection = _connectionFactory.CreateConnection();
        var id = await connection.ExecuteScalarAsync<int>(sql, contract);
        return id;
    }

    public async Task<int?> GetStatusIdByNameAsync(string statusName)
    {
        const string sql = @"SELECT Id FROM ContractStatuses WHERE StatusName = @StatusName;";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<int?>(sql, new { StatusName = statusName });
    }

    public async Task UpdateAsync(Contract contract)
    {
        const string sql = @"
            UPDATE Contracts
            SET CompanyId = @CompanyId,
                EmployeeId = @EmployeeId,
                Position = @Position,
                Description = @Description,
                StartDate = @StartDate,
                EndDate = @EndDate,
                Wage = @Wage,
                ContractStatusId = @ContractStatusId
            WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, contract);
    }

    public async Task DeleteAsync(int id)
    {
        const string sql = @"DELETE FROM Contracts WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}