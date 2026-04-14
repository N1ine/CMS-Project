using MediatR;

namespace Application.DTOs.Commands;

public class CreateEmployeeCommand : IRequest<int>
{
    public int CompanyId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
}