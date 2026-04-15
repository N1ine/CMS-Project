using Shared.DTOs;
using Shared.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface ICompanyService
{
    Task<CompanyDto> GetByIdAsync(int id);
    Task<IReadOnlyList<CompanyDto>> GetAllAsync();
    Task<int> CreateAsync(CreateCompanyCommand command);
    Task UpdateAsync(UpdateCompanyCommand command);
    Task DeleteAsync(int id);
}
