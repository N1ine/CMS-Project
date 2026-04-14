using Domain.Enums;

namespace Application.DTOs;

public record ContractDto(
    int Id,
    int CompanyId,
    int EmployeeId,
    string Position,
    string Description,
    DateTime StartDate,
    DateTime? EndDate,
    decimal Wage,
    ContractStatus Status,
    string EmployeeFirstName,
    string EmployeeLastName,
    string CompanyName
);