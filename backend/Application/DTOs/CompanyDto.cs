namespace Application.DTOs;
public record CompanyDto(
    int Id,
    string Name,
    string? Address,
    string? TaxNumber
);