using Application.DTOs;
using Application.DTOs.Commands;
using Application.DTOs.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEmployeeService
{
    Task<EmployeeDto> GetByIdAsync(int id);
    Task<IReadOnlyList<EmployeeDto>> GetByCompanyAsync(GetEmployeesByCompanyQuery query);
    Task<IReadOnlyList<EmployeeDto>> SearchByNameAsync(SearchEmployeesByNameQuery query);
    Task<int> CreateAsync(CreateEmployeeCommand command);
    Task UpdateAsync(UpdateEmployeeCommand command);
    Task DeleteAsync(int id);
}