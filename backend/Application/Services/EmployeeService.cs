using Application.DTOs;
using Application.DTOs.Commands;
using Application.DTOs.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services;
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICompanyRepository _companyRepository;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        ICompanyRepository companyRepository)
    {
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
    }

    public async Task<EmployeeDto> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ValidationException("Id must be greater than 0");

        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
            throw new EntityNotFoundException($"Employee with id {id} not found");

        return MapToDto(employee);
    }

    public async Task<IReadOnlyList<EmployeeDto>> GetByCompanyAsync(GetEmployeesByCompanyQuery query)
    {
        if (query is null)
            throw new ValidationException("Query must not be null");
        if (query.CompanyId <= 0)
            throw new ValidationException("CompanyId must be greater than 0");

        var company = await _companyRepository.GetByIdAsync(query.CompanyId);
        if (company == null)
            throw new EntityNotFoundException($"Company with id {query.CompanyId} not found");

        var employees = await _employeeRepository.GetByCompanyIdAsync(query.CompanyId);
        var result = new List<EmployeeDto>();
        foreach (var e in employees)
            result.Add(MapToDto(e));
        return result;
    }

    public async Task<IReadOnlyList<EmployeeDto>> SearchByNameAsync(SearchEmployeesByNameQuery query)
    {
        if (query is null)
            throw new ValidationException("Query must not be null");

        var firstName = query.FirstName?.Trim();
        var lastName = query.LastName?.Trim();

        //if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
           // throw new ValidationException("At least one of FirstName or LastName must be provided");

        var employees = await _employeeRepository.SearchByNameAsync(firstName, lastName);
        var result = new List<EmployeeDto>();
        foreach (var e in employees)
            result.Add(MapToDto(e));
        return result;
    }

    public async Task<int> CreateAsync(CreateEmployeeCommand command)
    {
        if (command is null)
            throw new ValidationException("Command must not be null");

        ValidateNames(command.FirstName, command.LastName);

        if (command.CompanyId <= 0)
            throw new ValidationException("CompanyId must be greater than 0");

        var company = await _companyRepository.GetByIdAsync(command.CompanyId);
        if (company == null)
            throw new EntityNotFoundException($"Company with id {command.CompanyId} not found");

        var employee = new Employee
        {
            CompanyId = command.CompanyId,
            FirstName = command.FirstName.Trim(),
            LastName = command.LastName.Trim(),
            Email = command.Email?.Trim()
        };

        return await _employeeRepository.CreateAsync(employee);
    }

    public async Task UpdateAsync(UpdateEmployeeCommand command)
    {
        if (command is null)
            throw new ValidationException("Command must not be null");

        if (command.Id <= 0)
            throw new ValidationException("Id must be greater than 0");

        ValidateNames(command.FirstName, command.LastName);

        if (command.CompanyId <= 0)
            throw new ValidationException("CompanyId must be greater than 0");

        var existing = await _employeeRepository.GetByIdAsync(command.Id);
        if (existing == null)
            throw new EntityNotFoundException($"Employee with id {command.Id} not found");

        var company = await _companyRepository.GetByIdAsync(command.CompanyId);
        if (company == null)
            throw new EntityNotFoundException($"Company with id {command.CompanyId} not found");

        existing.CompanyId = command.CompanyId;
        existing.FirstName = command.FirstName.Trim();
        existing.LastName = command.LastName.Trim();
        existing.Email = command.Email?.Trim();

        await _employeeRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0)
            throw new ValidationException("Id must be greater than 0");

        var existing = await _employeeRepository.GetByIdAsync(id);
        if (existing == null)
            throw new EntityNotFoundException($"Employee with id {id} not found");

        await _employeeRepository.DeleteAsync(id);
    }

    private static void ValidateNames(string? firstName, string? lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ValidationException("FirstName must not be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ValidationException("LastName must not be empty");
    }

    private static EmployeeDto MapToDto(Employee employee) =>
        new EmployeeDto(
            employee.Id,
            employee.CompanyId,
            employee.FirstName,
            employee.LastName,
            employee.Email
        );
}
