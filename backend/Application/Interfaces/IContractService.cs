using Application.DTOs.Requests;
using Application.DTOs.Commands;
using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IContractService
{
    Task<ContractDto> GetByIdAsync(int id);
    Task<IReadOnlyList<ContractDto>> GetAllAsync();

    Task<IReadOnlyList<ContractDto>> SearchByPositionAsync(SearchContractsByPositionQuery query);
    Task<IReadOnlyList<ContractDto>> SearchByEmployeeNameAsync(SearchContractsByEmployeeNameQuery query);
    Task<IReadOnlyList<ContractDto>> GetByStatusAsync(GetContractsByStatusQuery query);
    Task<IReadOnlyList<ContractDto>> GetByDateRangeAsync(GetContractsByDateRangeQuery query);

    Task<int> CreateAsync(CreateContractCommand command);
    Task UpdateAsync(UpdateContractCommand command);
    Task DeleteAsync(DeleteContractCommand command);
}