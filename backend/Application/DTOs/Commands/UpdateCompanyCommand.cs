using MediatR;

namespace Application.DTOs.Commands;

public class UpdateCompanyCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? TaxNumber { get; set; }
}