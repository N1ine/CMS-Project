using Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Domain.Interfaces;
public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(int id);
    Task<IEnumerable<Company>> GetAllAsync();

    Task<int> CreateAsync(Company company);
    Task UpdateAsync(Company company);
    Task DeleteAsync(int id);
}