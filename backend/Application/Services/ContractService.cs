using Application.DTOs;
using Application.DTOs.Commands;
using Application.DTOs.Requests;
using Domain.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Enums;

namespace Application.Services;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICompanyRepository _companyRepository;

    public ContractService(
        IContractRepository contractRepository,
        IEmployeeRepository employeeRepository,
        ICompanyRepository companyRepository)
    {
        _contractRepository = contractRepository;
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
    }

    public async Task<ContractDto> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ValidationException("Id must be greater than 0");

        var contract = await _contractRepository.GetByIdAsync(id);

        if (contract == null)
            throw new EntityNotFoundException($"Contract with id {id} not found");

        return await MapToDtoAsync(contract);
    }

    public async Task<IReadOnlyList<ContractDto>> GetAllAsync()
    {
        var contracts = await _contractRepository.GetAllAsync();

        return await MapListToDtoAsync(contracts);
    }



    public async Task<IReadOnlyList<ContractDto>> SearchByPositionAsync(SearchContractsByPositionQuery query)
    {
        if (query is null)
            throw new ValidationException("Query must not be null");

        if (string.IsNullOrWhiteSpace(query.Position))
            throw new ValidationException("Position must not be empty");

        var contracts = await _contractRepository.SearchByPositionAsync(query.Position.Trim());
        return await MapListToDtoAsync(contracts);
    }
    public async Task<IReadOnlyList<ContractDto>> SearchByEmployeeNameAsync(SearchContractsByEmployeeNameQuery query)
    {
        if (query is null)
            throw new ValidationException("Query must not be null");

        var firstName = query.FirstName?.Trim();
        var lastName = query.LastName?.Trim();

        if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
            throw new ValidationException("At least one of FirstName or LastName must be provided");

        var contracts = await _contractRepository.SearchByEmployeeNameAsync(firstName, lastName);
        return await MapListToDtoAsync(contracts);
    }

    public async Task<IReadOnlyList<ContractDto>> GetByStatusAsync(GetContractsByStatusQuery query)
    {
        if (query is null)
            throw new ValidationException("Query must not be null");

        var contracts = await _contractRepository.GetByStatusAsync(query.Status);
        return await MapListToDtoAsync(contracts);
    }



    public async Task<IReadOnlyList<ContractDto>> GetByDateRangeAsync(GetContractsByDateRangeQuery query)
    {
        if (query is null)
            throw new ValidationException("Query must not be null");

        if (query.StartDateFrom.HasValue && query.StartDateTo.HasValue &&
            query.StartDateFrom > query.StartDateTo)
        {
            throw new ValidationException("StartDateFrom must be less than or equal to StartDateTo");
        }

        if (query.EndDateFrom.HasValue && query.EndDateTo.HasValue &&
            query.EndDateFrom > query.EndDateTo)
        {
            throw new ValidationException("EndDateFrom must be less than or equal to EndDateTo");
        }

        var contracts = await _contractRepository.GetByDateRangeAsync(
            query.StartDateFrom,
            query.StartDateTo,
            query.EndDateFrom,
            query.EndDateTo);

        return await MapListToDtoAsync(contracts);
    }



    public async Task<int> CreateAsync(CreateContractCommand command)
    {
        if (command is null)
            throw new ValidationException("Command must not be null");

        ValidateContractCore(command.Position, command.Description, command.Wage);
        ValidateContractDates(command.StartDate, command.EndDate);

        var company = await _companyRepository.GetByIdAsync(command.CompanyId);

        if (company == null)
            throw new EntityNotFoundException($"Company with id {command.CompanyId} not found");

        var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId);

        if (employee == null)
            throw new EntityNotFoundException($"Employee with id {command.EmployeeId} not found");

        var contract = new Contract
        {
            CompanyId = command.CompanyId,
            EmployeeId = command.EmployeeId,
            Position = command.Position.Trim(),
            Description = command.Description.Trim(),
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            Wage = command.Wage
        };

        var statusName = contract.Status switch
        {
            ContractStatus.NotStarted => "Not Started",
            ContractStatus.Active => "Active",
            ContractStatus.Finished => "Finished",
            _ => "Active"
        };

        var statusId = await _contractRepository.GetStatusIdByNameAsync(statusName);
        if (!statusId.HasValue)
            throw new ValidationException($"Contract status '{statusName}' not found in database. Please seed ContractStatuses.");

        contract.ContractStatusId = statusId.Value;

        return await _contractRepository.CreateAsync(contract);
    }

    public async Task UpdateAsync(UpdateContractCommand command)
    {
        if (command is null)
            throw new ValidationException("Command must not be null");

        if (command.Id <= 0)
            throw new ValidationException("Id must be greater than 0");

        ValidateContractCore(command.Position, command.Description, command.Wage);
        ValidateContractDates(command.StartDate, command.EndDate);

        var existing = await _contractRepository.GetByIdAsync(command.Id);

        if (existing == null)
            throw new EntityNotFoundException($"Contract with id {command.Id} not found");

        var company = await _companyRepository.GetByIdAsync(command.CompanyId);

        if (company == null)
            throw new EntityNotFoundException($"Company with id {command.CompanyId} not found");

        var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId);

        if (employee == null)
            throw new EntityNotFoundException($"Employee with id {command.EmployeeId} not found");

        existing.CompanyId = command.CompanyId;
        existing.EmployeeId = command.EmployeeId;
        existing.Position = command.Position.Trim();
        existing.Description = command.Description.Trim();
        existing.StartDate = command.StartDate;
        existing.EndDate = command.EndDate;
        existing.Wage = command.Wage;

        await _contractRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(DeleteContractCommand command)
    {
        if (command is null)
            throw new ValidationException("Command must not be null");

        if (command.Id <= 0)
            throw new ValidationException("Id must be greater than 0");

        var existing = await _contractRepository.GetByIdAsync(command.Id);

        if (existing == null)
            throw new EntityNotFoundException($"Contract with id {command.Id} not found");

        await _contractRepository.DeleteAsync(command.Id);
    }

    private void ValidateContractCore(string? position, string? description, decimal wage)
    {

        if (string.IsNullOrWhiteSpace(position))
            throw new ValidationException("Position must not be empty");

        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException("Description must not be empty");

        if (wage <= 0)
            throw new ValidationException("Wage must be greater than 0");
    }

    private void ValidateContractDates(DateTime start, DateTime? end)
    {
        if (end.HasValue && end.Value < start)
            throw new ValidationException("EndDate must be greater than or equal to StartDate");
    }

    private async Task<ContractDto> MapToDtoAsync(Contract contract)
    {
        var employee = await _employeeRepository.GetByIdAsync(contract.EmployeeId)
                       ?? throw new EntityNotFoundException($"Employee with id {contract.EmployeeId} not found");

        var company = await _companyRepository.GetByIdAsync(contract.CompanyId)
                      ?? throw new EntityNotFoundException($"Company with id {contract.CompanyId} not found");

        return new ContractDto(
            contract.Id,
            contract.CompanyId,
            contract.EmployeeId,
            contract.Position,
            contract.Description,
            contract.StartDate,
            contract.EndDate,
            contract.Wage,
            contract.Status,
            employee.FirstName,
            employee.LastName,
            company.Name
        );

    }

    private async Task<IReadOnlyList<ContractDto>> MapListToDtoAsync(IEnumerable<Contract> contracts)
    {
        var result = new List<ContractDto>();

        foreach (var c in contracts)
            result.Add(await MapToDtoAsync(c));

        return result;
    }
}