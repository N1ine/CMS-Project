using MediatR;

namespace Application.DTOs.Commands;

public class CreateCompanyCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? TaxNumber { get; set; }
}
