using Application.DTOs;
using Application.DTOs.Commands;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services;
public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<CompanyDto> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ValidationException("Id must be greater than 0");

        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null)
            throw new EntityNotFoundException($"Company with id {id} not found");

        return MapToDto(company);
    }

    public async Task<IReadOnlyList<CompanyDto>> GetAllAsync()
    {
        var companies = await _companyRepository.GetAllAsync();
        var result = new List<CompanyDto>();
        foreach (var c in companies)
            result.Add(MapToDto(c));
        return result;
    }

    public async Task<int> CreateAsync(CreateCompanyCommand command)
    {
        if (command is null)
            throw new ValidationException("Command must not be null");

        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ValidationException("Company name must not be empty");

        var company = new Company
        {
            Name = command.Name.Trim(),
            Address = command.Address?.Trim(),
            TaxNumber = command.TaxNumber?.Trim()
        };

        return await _companyRepository.CreateAsync(company);
    }

    public async Task UpdateAsync(UpdateCompanyCommand command)
    {
        if (command is null)
            throw new ValidationException("Command must not be null");

        if (command.Id <= 0)
            throw new ValidationException("Id must be greater than 0");

        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ValidationException("Company name must not be empty");

        var existing = await _companyRepository.GetByIdAsync(command.Id);
        if (existing == null)
            throw new EntityNotFoundException($"Company with id {command.Id} not found");

        existing.Name = command.Name.Trim();
        existing.Address = command.Address?.Trim();
        existing.TaxNumber = command.TaxNumber?.Trim();

        await _companyRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0)
            throw new ValidationException("Id must be greater than 0");

        var existing = await _companyRepository.GetByIdAsync(id);
        if (existing == null)
            throw new EntityNotFoundException($"Company with id {id} not found");

        await _companyRepository.DeleteAsync(id);
    }

    private static CompanyDto MapToDto(Company company) =>
        new CompanyDto(
            company.Id,
            company.Name,
            company.Address,
            company.TaxNumber
        );
}