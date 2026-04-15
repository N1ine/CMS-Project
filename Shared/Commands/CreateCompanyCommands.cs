using Shared.DTOs;
using Shared.Enums;
using MediatR;

namespace Shared.Commands;

public class CreateCompanyCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? TaxNumber { get; set; }
}
