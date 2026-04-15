using Domain.Entities;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces;
public interface IContractRepository
{
    Task<Contract?> GetByIdAsync(int id);

    Task<IEnumerable<Contract>> GetAllAsync(int? employeeId = null);

    Task<IEnumerable<Contract>> SearchByPositionAsync(string position);

    Task<int?> GetStatusIdByNameAsync(string statusName);

    Task<IEnumerable<Contract>> SearchByEmployeeNameAsync(string? firstName, string? lastName);

    Task<IEnumerable<Contract>> GetByStatusAsync(ContractStatus status);

    Task<IEnumerable<Contract>> GetByEmployeeIdAsync(int employeeId);

    Task<IEnumerable<Contract>> GetByDateRangeAsync(DateTime? startDateFrom,
                                                    DateTime? startDateTo,
                                                    DateTime? endDateFrom,
                                                    DateTime? endDateTo);

    Task<int> CreateAsync(Contract contract);
    Task UpdateAsync(Contract contract);
    Task DeleteAsync(int id);
}
